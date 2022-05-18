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

namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for managing already registered users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase {

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
        public RegisteredUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {

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
        public async Task<IActionResult> LogIn([Bind("username,password")] RegisteredUser user) {

            if (await _registeredUsersService.verifyUser(user.username, user.password) == true) {

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
        public async Task<IActionResult> LogIn(string? username) {

            if (await _registeredUsersService.doesUserExists(username) == true) {

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
        /// Logs in the user if their email and password match.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("emailLogIn")]
        public async Task<IActionResult> emailLogIn([Bind("email,password")] RegisteredUser user) {

            if (await _registeredUsersService.doEmailAndPasswordMAtch(user.email, user.password) == true) {

                user = await _registeredUsersService.GetRegisteredUserByEmail(user.email);

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
        /// logs out user.
        /// </summary>
        /// <returns></returns>
        [HttpPut("logOut")]
        [Authorize]
        public async Task<IActionResult> logOut() {

            string? username = User.FindFirst("username")?.Value;
            await _refreshTokenService.RemovePreviousTokens(username, Request.Headers["User-Agent"].ToString());
            return Ok();
        }
        
        /// <summary>
        /// Finishes the sign up process of a user and adds them to the database as a registered user.
        /// </summary>
        /// <returns>200 and refresh token on success, BadRequest otherwise</returns>
        [HttpPost("signUp")]
        [Authorize]
        public async Task<IActionResult> FinishSignUp() {

            string? username = User.FindFirst("username")?.Value;
            // Really should absolutely never happen, short of an attack.
            if (username == null) {
                return BadRequest();
            }

            PendingUser? userToSignUp = await _pendingUsersService.GetPendingUserWithSecretQuestion(username);
            // Also should absolutely never happen short of an attack.
            if (userToSignUp == null) {
                return BadRequest();
            }

            // Also should always be false, short of an attack.
            if (await _registeredUsersService.doesUserExists(username) == false) {

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

        /// <summary>
        /// gets nickname of specific user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("getNickName/{username}")]
        [Authorize]
        public async Task<IActionResult> getNickName(string? username) {
            if (username == null) {
                return BadRequest();
            }
            RegisteredUser? user = await _registeredUsersService.GetRegisteredUser(username);
            if (user == null) {
                return NotFound();
            }
            else {
                return Ok(user.nickname);
            }
        }
        
        /// <summary>
        /// gets description of user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("getDescription/{username}")]
        [Authorize]
        public async Task<IActionResult> getDescription(string? username) {
            if (username == null) {
                return BadRequest();
            }
            RegisteredUser? user = await _registeredUsersService.GetRegisteredUser(username);
            if (user == null) {
                return NotFound();
            }
            else {
                return Ok(user.description);
            }
        }

        /// <summary>
        /// gets NickNum of user.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getNickNum")]
        [Authorize]
        public async Task<IActionResult> getNickNum() {
            string? currentUser = User.FindFirst("username")?.Value;
            RegisteredUser? user = await _registeredUsersService.GetRegisteredUser(currentUser);
            if (user == null) {
                return NotFound();
            }
            else {
                return Ok(user.nickNum);
            }
        }


        /// <summary>
        /// Checks if user exists by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("doesUserExistByUsername/{username}")]
        [Authorize]
        public async Task<bool> doesUserExistByUsername(string? username) {
            string? currentUser = User.FindFirst("username")?.Value;
            return await _registeredUsersService.doesUserExists(username);
        }

        /// <summary>
        /// checks if user exists by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("doesUserExistByEmail/{email}")]
        [Authorize]
        public async Task<bool> doesUserExistByEmail(string? email) {
            string? currentUser = User.FindFirst("username")?.Value;
            return await _registeredUsersService.doesUserExistsByEmail(email);
        }

        /// <summary>
        /// checks if user exists by phone.
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("doesUserExistByPhone/{phone}")]
        [Authorize]
        public async Task<bool> doesUserExistByPhone(string? phone) {
            string? currentUser = User.FindFirst("username")?.Value;
            return await _registeredUsersService.doesUserExistsByPhone(phone);
        }

        /// <summary>
        /// changes user's password.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("editPassword")]
        [Authorize]
        public async Task<IActionResult> updatePassword([Bind("password")] RegisteredUser user) {
            if (user == null) {
                return BadRequest();
            }
            string? currentUser = User.FindFirst("username")?.Value;
            bool result = await _registeredUsersService.updatePassword(currentUser, user.password);
            if (!result) { return BadRequest(); }
            return NoContent();
        }

        /// <summary>
        /// changes nickname of user.
        /// </summary>
        /// <param name="newNickName"></param>
        /// <returns></returns>
        [HttpPut("editNickName/{newNickName}")]
        [Authorize]
        public async Task<IActionResult> editNickName(string? newNickName) {
            if (newNickName == null) {
                return BadRequest();
            }
            string? currentUser = User.FindFirst("username")?.Value;
            bool result = await _registeredUsersService.editNickName(currentUser, newNickName);
            if (!result) { return BadRequest(); }
            return NoContent();
        }

        /// <summary>
        /// changes description of user.
        /// </summary>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("editDescription/{newDescription}")]
        public async Task<IActionResult> editDescription(string? newDescription) {

            if (newDescription == null) {
                return BadRequest();
            }
            string currentUser = User.FindFirst("username")?.Value;
            bool result = await _registeredUsersService.editDescription(currentUser, newDescription);
            if (!result) {
                return BadRequest();
            }
            return NoContent();
        }

        /// <summary>
        /// Gets secret question of user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpGet("secretQuestion/{username}")]
        public async Task<bool> verifySecretQuestion(string? username, string? question, string? answer) {

            if (username == null || question == null || answer == null) {
                return false;
            }

            bool result = await _registeredUsersService.verifySecretQuestion(username, question, answer);
            return result;
        }

        /// <summary>
        /// Renews vverification code of specific user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("renewVerificationCode/{username}")]
        public async Task<IActionResult> renewVerificationCode(string? username) {
            if (username == null) {
                return BadRequest();
            }
            bool success = await _registeredUsersService.generateVerificationCode(username, 
                new MailRequest() {
                    Email = _configuration["MailSettings:Mail"],
                    Password = _configuration["MailSettings:Password"],
                    Subject = "Your verification code",
                    Host = _configuration["MailSettings:Host"],
                    Port = Int32.Parse(_configuration["MailSettings:Port"])
                });
            if (success) {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Checks if the code matches and user and if so, gives user access token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        [HttpGet("verifyCode/{username}")]
        public async Task<IActionResult> verifyCode(string? username, string? verificationCode) {

            if (username == null || verificationCode == null) {
                return BadRequest();
            }
            bool result = await _registeredUsersService.verifyVerificationCode(username, verificationCode);
            if (result) {
                return Ok(_tokenGenerator.GenerateAccessToken(username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"]
                    ));
            }
            return BadRequest();
        }
    }
}
