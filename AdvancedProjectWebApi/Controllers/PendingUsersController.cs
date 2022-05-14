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

        [HttpPost]
        public async Task<IActionResult> signUp([Bind("username,password,phone,email," +
            "nickname,secretQuestion")] PendingUser pendingUser) {

            if (ModelState.IsValid) {
                if (await _pendingUsersService.doesUserExist(pendingUser.username) == false) {

                    MailRequest mail = new MailRequest() {
                        Email = config["MailSettings:Mail"],
                        Password = config["MailSettings:Password"],
                        Subject = "Your verification code",
                        ToEmail = pendingUser.email,
                        Host = config["MailSettings:Host"],
                        Port = Int32.Parse(config["MailSettings:Port"])
                    };

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
