using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Data;
using Services;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase {

        private readonly IRegisteredUsersService _registeredUsersService;

        public RegisteredUsersController(AdvancedProgrammingProjectsServerContext context) {

            this._registeredUsersService = new DatabaseRegisteredUsersService(context);

        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string? username) {
            if (await _registeredUsersService.doesUserExists(username) == true) {

                HttpContext.Session.SetString("username", username);
                return Ok();
            }
            return NotFound();
        }
    }
}
