using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Data;
using Services.TokenServices.Implementations;
using Services.TokenServices.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;


namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [RequireHttps]
    public class PendingUsersController : ControllerBase {

        private readonly IPendingUsersService _pendingUsersService;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAccessTokenGenerator _authTokenGenerator;

        public PendingUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {

            this._pendingUsersService = new DatabasePendingUsersService(context);
            this._refreshTokenService = new RefreshTokenService(context);
            this._authTokenGenerator = new AuthTokenGenerator();
            this._configuration = config;
        }

        private MailRequest createEmail(string emailTo) {
            MailRequest mail = new MailRequest() {
                Email = _configuration["MailSettings:Mail"],
                Password = _configuration["MailSettings:Password"],
                Subject = "Your verification code",
                ToEmail = emailTo,
                Host = _configuration["MailSettings:Host"],
                Port = Int32.Parse(_configuration["MailSettings:Port"])
            };
            return mail;
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> renewEmail(string? username) {
            if (username == null) {
                return BadRequest();
            }
            PendingUser? user = await _pendingUsersService.GetPendingUser(username);
            if (user != null) {
                MailRequest mail = this.createEmail(user.email);
                await _pendingUsersService.RenewCode(user, mail);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> signUp([Bind("username,password,phone,email," +
            "nickname,secretQuestion")] PendingUser pendingUser) {

            if (ModelState.IsValid) {
                if (await _pendingUsersService.doesUserExist(pendingUser.username) == false) {

                    MailRequest mail = this.createEmail(pendingUser.email);

                    await _pendingUsersService.addToPending(pendingUser, "SHA256", mail);
                    return CreatedAtAction("signUp", new { });
                }
                else {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> verifyCode(string? username, string? verificationCode) {
            if (username == null || verificationCode == null) {
                return BadRequest();
            }
            PendingUser? user = await _pendingUsersService.GetPendingUser(username);
            // On success, give the user the Json token they need for logging in (they will be auto logged in).
            if (await _pendingUsersService.canVerify(user, verificationCode)) {

                return Ok(_authTokenGenerator.GenerateAccessToken(username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"],
                    20));
            };
            return BadRequest();
        }
    }
}
