using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Data;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase {

        public IConfiguration _configuration;

        private readonly IRegisteredUsersService _registeredUsersService;
        private readonly IPendingUsersService _pendingUsersService;

        public RegisteredUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {

            this._registeredUsersService = new DatabaseRegisteredUsersService(context);
            this._pendingUsersService = new DatabasePendingUsersService(context);
            this._configuration = config;

        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string? username) {

            if (await _registeredUsersService.doesUserExists(username) == true) {

                JwtSecurityToken token = Utils.Utils.generateJwtToken(username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"]);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
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
                return Ok();
            }
            return BadRequest();
        }
    }
}
