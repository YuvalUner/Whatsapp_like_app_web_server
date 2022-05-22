using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Data;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;

namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for adding messages from other servers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // Ideally, should require this. But we don't know what the testers will be running.
    // [RequireHttps]
    public class transferController : ControllerBase {

        private readonly IContactsService _contactsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public transferController(IContactsService contactsService) {
            this._contactsService = contactsService;
        }

        /// <summary>
        /// Adds a message from another server.
        /// </summary>
        /// <param name="transfer">A message listing from who, to whom and its content</param>
        /// <returns>201 on success, 404 if user not found, 401 otherwise.</returns>
        [HttpPost]
        public async Task<IActionResult> transfer([Bind("from,to,content")] Transfer transfer) {

            if (ModelState.IsValid) {
                bool result = await _contactsService.addMessage(transfer.to, transfer.from, new Message {
                    content = transfer.content,
                    sent = false,
                    type = "text",
                    created = DateTime.Now
                });
                if (result == true) {
                    return CreatedAtAction("transfer", new { });
                }
                else {
                    return NotFound();
                }
            }
            return BadRequest();
        }
    }
}
