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
        public async Task<IActionResult> invite([Bind("to,from,server")] Invite invite) {


            if (ModelState.IsValid) {

                bool success = await _contactsService.addContact(invite.to, new Contact {
                    contactOf = invite.to,
                    id = invite.from,
                    last = null,
                    lastdate = DateTime.Now,
                    server = invite.server
                });
                if (success) {
                    return CreatedAtAction("invite", new { });
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
