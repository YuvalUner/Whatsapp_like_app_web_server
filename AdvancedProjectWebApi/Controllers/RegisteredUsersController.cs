using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Data;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;
using Services.TokenServices.Interfaces;
using Services.TokenServices.Implementations;

namespace AdvancedProjectWebApi.Controllers
{

    /// <summary>
    /// A controller for managing already registered users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase
    {

        public IConfiguration _configuration;

        private readonly IRegisteredUsersService _registeredUsersService;
        private readonly IPendingUsersService _pendingUsersService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IHybridTokenGenerator _tokenGenerator;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public RegisteredUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config)
        {

            this._registeredUsersService = new DatabaseRegisteredUsersService(context);
            this._pendingUsersService = new DatabasePendingUsersService(context);
            this._refreshTokenService = new RefreshTokenService(context);
            this._tokenGenerator = new AuthTokenGenerator();
            this._configuration = config;

        }

        /// <summary>
        /// Logs in the user if their username and password match.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>200 with access and refresh tokens on success, BadRequest otherwise.</returns>
        [HttpPost]
        public async Task<IActionResult> LogIn([Bind("username,password")] RegisteredUser user)
        {

            if (await _registeredUsersService.verifyUser(user.username, user.password) == true)
            {

                AuthToken token = _tokenGenerator.GenerateAuthToken(user.username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"]);
                string? userAgent = Request.Headers["User-Agent"].ToString();
                await _refreshTokenService.RemovePreviousTokens(user.username, userAgent);
                await _refreshTokenService.storeRefreshToken(token.RefreshToken, user.username, userAgent);

                return Ok(token);
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove later.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("testingOnlyRemoveLater")]
        public async Task<IActionResult> LogIn(string? username)
        {

            if (await _registeredUsersService.doesUserExists(username) == true)
            {

                AuthToken token = _tokenGenerator.GenerateAuthToken(username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"]);
                string? userAgent = Request.Headers["User-Agent"].ToString();
                await _refreshTokenService.RemovePreviousTokens(username, userAgent);
                await _refreshTokenService.storeRefreshToken(token.RefreshToken, username, userAgent);

                return Ok(token);
            }
            return BadRequest();
        }

        /// <summary>
        /// Finishes the sign up process of a user and adds them to the database as a registered user.
        /// </summary>
        /// <returns>200 and refresh token on success, BadRequest otherwise</returns>
        [HttpPost("signUp")]
        [Authorize]
        public async Task<IActionResult> FinishSignUp()
        {

            string? username = User.FindFirst("username")?.Value;
            // Really should absolutely never happen, short of an attack.
            if (username == null)
            {
                return BadRequest();
            }

            PendingUser? userToSignUp = await _pendingUsersService.GetPendingUserWithSecretQuestion(username);
            // Also should absolutely never happen short of an attack.
            if (userToSignUp == null)
            {
                return BadRequest();
            }

            // Also should always be false, short of an attack.
            if (await _registeredUsersService.doesUserExists(username) == false)
            {

                await _registeredUsersService.addNewRegisteredUser(userToSignUp);
                await _pendingUsersService.RemovePendingUser(userToSignUp);

                string? userAgent = Request.Headers["User-Agent"].ToString();
                string rToken = _tokenGenerator.GenerateRefreshToken();
                await _refreshTokenService.RemovePreviousTokens(username, userAgent);
                await _refreshTokenService.storeRefreshToken(rToken, username, userAgent);

                return Ok(rToken);
            }
            return BadRequest();
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> getNickName(string? username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            string? currentUser = User.FindFirst("username")?.Value;
            RegisteredUser? user = await _registeredUsersService.GetRegisteredUser(currentUser);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user.nickname);
            }
        }

        [HttpGet("getNickNum")]
        [Authorize]
        public async Task<IActionResult> getNickNum()
        {
            string? currentUser = User.FindFirst("username")?.Value;
            RegisteredUser? user = await _registeredUsersService.GetRegisteredUser(currentUser);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user.nickNum);
            }
        }

        [HttpPut("/editPassword")]
        [Authorize]
        public async Task<IActionResult> updatePassword(string? newPassword)
        {
            if (newPassword == null)
            {
                return BadRequest();
            }
            string? currentUser = User.FindFirst("username")?.Value;
            bool result = await _registeredUsersService.updatePassword(currentUser, newPassword);
            if (!result) { return BadRequest(); }
            return NoContent();
        }

        [HttpPut("/editNickName")]
        [Authorize]
        public async Task<IActionResult> editNickName(string? newNickName)
        {
            if (newNickName == null)
            {
                return BadRequest();
            }
            string? currentUser = User.FindFirst("username")?.Value;
            bool result = await _registeredUsersService.editNickName(currentUser, newNickName);
            if (!result) { return BadRequest(); }
            return NoContent();
        }

    }
}
