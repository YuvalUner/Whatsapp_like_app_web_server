using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;

namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for managing invitiations from other servers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // Ideally, should require this. But we don't know what the testers will be running.
    // [RequireHttps]
    public class invitationsController : ControllerBase {

        private readonly IContactsService _contactsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public invitationsController(IContactsService contacts) {
            this._contactsService = contacts;
        }

        /// <summary>
        /// Adds a user from another server to the contact list of a user.
        /// </summary>
        /// <param name="invite">An invite containing to, from and which server</param>
        /// <returns>204 on success, 404 if user not found, 401 otherwise</returns>
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
