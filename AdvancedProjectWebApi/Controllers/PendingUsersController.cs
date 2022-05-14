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

        public PendingUsersController(AdvancedProgrammingProjectsServerContext context) {

            this._pendingUsersService = new DatabasePendingUsersService(context);

        }

        [HttpPost]
        public async Task<IActionResult> signUp([Bind("username,password,phone,email," +
            "nickname,secretQuestion")] PendingUser pendingUser) {

            if (ModelState.IsValid) {
                await _pendingUsersService.addToPending(pendingUser, "SHA256");
            }
            return BadRequest();
        }
    }
}
