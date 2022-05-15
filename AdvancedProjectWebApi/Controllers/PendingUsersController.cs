using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Data;
using Services;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [RequireHttps]
    public class PendingUsersController : ControllerBase {

        private readonly IPendingUsersService _pendingUsersService;
        private readonly IConfiguration config;

        public PendingUsersController(AdvancedProgrammingProjectsServerContext context, IConfiguration config) {

            this._pendingUsersService = new DatabasePendingUsersService(context);
            this.config = config;

        }

        private MailRequest createEmail(string emailTo) {
            MailRequest mail = new MailRequest() {
                Email = config["MailSettings:Mail"],
                Password = config["MailSettings:Password"],
                Subject = "Your verification code",
                ToEmail = emailTo,
                Host = config["MailSettings:Host"],
                Port = Int32.Parse(config["MailSettings:Port"])
            };
            return mail;
        }

        [HttpPost("{username}")]
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
    }
}
