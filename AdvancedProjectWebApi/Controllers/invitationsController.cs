using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Services;
using Data;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class invitationsController : ControllerBase {

        private readonly IContactsService _contactsService;

        public invitationsController(AdvancedProgrammingProjectsServerContext context) {
            this._contactsService = new DatabaseContactsService(context);
        }

        [HttpPost]
        public async Task<IActionResult> invite(string from, string to, string server) {

            if (from == null || to == null || server == null) {
                return BadRequest();
            }

            bool success = await _contactsService.addContact(to, new Contact {
                contactOf = to,
                id = from,
                last = null,
                lastdate = DateTime.Now,
                server = server
            });
            if (success) {
                return NoContent();
            }
            return NotFound();
        }
    }
}
