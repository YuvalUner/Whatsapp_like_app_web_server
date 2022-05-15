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
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase {

        public IConfiguration _configuration;

        private readonly IRegisteredUsersService _registeredUsersService;
        private readonly IPendingUsersService _pendingUsersService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthTokenGenerator _authTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;

        public RegisteredUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {

            this._registeredUsersService = new DatabaseRegisteredUsersService(context);
            this._pendingUsersService = new DatabasePendingUsersService(context);
            this._refreshTokenService = new RefreshTokenService(context);
            this._authTokenGenerator = new AuthTokenGenerator();
            this._refreshTokenGenerator = new AuthTokenGenerator();
            this._configuration = config;

        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string? username) {

            if (await _registeredUsersService.doesUserExists(username) == true) {

                return Ok(_authTokenGenerator.GenerateAuthToken(username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"],
                    20));
            }
            return BadRequest();
        }

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
                RefreshToken rToken = new RefreshToken() {
                    Token = _refreshTokenGenerator.GenerateRefreshToken(),
                    ExpiryDate = DateTime.UtcNow.AddDays(30),
                    RegisteredUserusername = username
                };
                await _refreshTokenService.storeRefreshToken(rToken);
                return Ok(rToken.Token);
            }
            return BadRequest();
        }
    }
}
