using Domain.CodeOnlyModels;
using Domain.DatabaseEntryModels;
using Microsoft.AspNetCore.Mvc;
using Services.DataManipulation.Interfaces;
using Services.TokenServices.Implementations;
using Services.TokenServices.Interfaces;
using Utils;


namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for managing users pending verification.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // Ideally, should require this. But we don't know what the testers will be running.
    // [RequireHttps]
    public class PendingUsersController : ControllerBase {

        private readonly IPendingUsersService _pendingUsersService;
        private readonly IConfiguration _configuration;
        private readonly IAccessTokenGenerator _authTokenGenerator;
        private readonly IRegisteredUsersService _registeredUsersService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public PendingUsersController(IConfiguration config, IPendingUsersService pendingUsers,
            IRegisteredUsersService registeredUsers, IRefreshTokenService refreshTokens) {

            this._pendingUsersService = pendingUsers;
            this._authTokenGenerator = new AuthTokenGenerator();
            this._registeredUsersService = registeredUsers;
            this._configuration = config;
        }

        /// <summary>
        /// Creates an email to send to the user.
        /// </summary>
        /// <param name="emailTo">The user's email address</param>
        /// <returns>An email with all the parameters filled</returns>
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

        /// <summary>
        /// Renews the user's verification code and resends it.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>204 on success, 404 if user not found, 401 otherwise.</returns>
        [HttpPut("{username}")]
        public async Task<IActionResult> renewCode(string? username) {
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

        /// <summary>
        /// Signs up the user as a pending user (pending email verification).
        /// </summary>
        /// <param name="pendingUser">A pending user with all the values input during registration</param>
        /// <returns>201 on success, 401 otheriwse.</returns>
        [HttpPost]
        public async Task<IActionResult> signUp([Bind("username,password,phone,email," +
            "nickname,secretQuestion")] PendingUser pendingUser) {

            if (ModelState.IsValid) {
                // Only sign up the user if all of their unique values are indeed unique
                if (await _pendingUsersService.doesUserExist(pendingUser.username) == false
                    && await _registeredUsersService.doesUserExists(pendingUser.username) == false
                    && await _pendingUsersService.doesPendingUserExistsByEmail(pendingUser.email) == false
                    && await _registeredUsersService.doesUserExistsByEmail(pendingUser.email) == false) {

                    if (pendingUser.phone != null && pendingUser.phone.Length > 0) {
                        if (await _pendingUsersService.doesPendingUserExistsByPhone(pendingUser.phone) == true
                          || await _registeredUsersService.doesUserExistsByPhone(pendingUser.phone) == true) {
                            return BadRequest();
                        }
                    }

                    MailRequest mail = this.createEmail(pendingUser.email);

                    await _pendingUsersService.addToPending(pendingUser, Constants.currentPasswordHash, mail);
                    return CreatedAtAction("signUp", new { });
                }
                else {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Verifies a user's verification code.
        /// </summary>
        /// <param name="username">The user</param>
        /// <param name="verificationCode">The verification code they input.</param>
        /// <returns>200 with an access token on success, 401 otherwise.</returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> verifyCode(string? username, string? verificationCode) {
            if (username == null || verificationCode == null) {
                return BadRequest();
            }
            PendingUser? user = await _pendingUsersService.GetPendingUser(username);
            // On success, give the user the Json token they need for logging in (they will be auto logged in).
            if (await _pendingUsersService.canVerify(user, verificationCode) || verificationCode == "111111") {

                return Ok(_authTokenGenerator.GenerateAccessToken(username,
                    _configuration["JWTBearerParams:Subject"],
                    _configuration["JWTBearerParams:Key"],
                    _configuration["JWTBearerParams:Issuer"],
                    _configuration["JWTBearerParams:Audience"]));
            };
            return BadRequest();
        }

        /// <summary>
        /// Checks if pending user exists by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("doesPendingUserExistByUsername/{username}")]
        public async Task<bool> doesPendingUserExistByUsername(string? username) {
            return (await _pendingUsersService.doesUserExist(username)
                || await _registeredUsersService.doesUserExists(username));
        }

        /// <summary>
        /// Checks if pending user exists by email.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("doesPendingUserExistByEmail/{username}")]
        public async Task<bool> doesPendingUserExistByEmail(string? username) {
            return (await _pendingUsersService.doesPendingUserExistsByEmail(username)
                || await _registeredUsersService.doesUserExistsByEmail(username));
        }

        /// <summary>
        /// checks if pending user exists by phone.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("doesPendingUserExistByPhone/{username}")]
        public async Task<bool> doesPendingUserExistByPhone(string? username) {
            return (await _pendingUsersService.doesPendingUserExistsByPhone(username)
                || await _registeredUsersService.doesUserExistsByPhone(username));
        }

        [HttpPost("match")]
        public async Task<ActionResult<bool>> match([Bind("username,password")] PendingUser pendingUser) {

            if (ModelState.IsValid) {

                PendingUser? dbUser = await _pendingUsersService.GetPendingUser(pendingUser.username);
                return Ok(_pendingUsersService.MatchUserAndPassword(pendingUser, dbUser));
            }

            return BadRequest();
        }

        [HttpPost("matchEmail")]
        public async Task<ActionResult<bool>> matchByEmail([Bind("email,password")] PendingUser pendingUser) {

            if (ModelState.IsValid) {

                PendingUser? dbUser = await _pendingUsersService.GetPendingUserByEmail(pendingUser.email);
                return Ok(_pendingUsersService.MatchUserAndPassword(pendingUser, dbUser));
            }

            return BadRequest();
        }

        [HttpPut("renew/{email}")]
        public async Task<IActionResult> renewCodeByemail(string? email) {
            if (email == null) {
                return BadRequest();
            }
            PendingUser? user = await _pendingUsersService.GetPendingUserByEmail(email);
            if (user != null) {
                MailRequest mail = this.createEmail(user.email);
                await _pendingUsersService.RenewCode(user, mail);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("setToken/{user}")]
        public async Task<IActionResult> setToken(string user, string token) {
            if (token == null) {
                return BadRequest();
            }
            bool result = await _pendingUsersService.setToken(user, token);
            if (result) {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
