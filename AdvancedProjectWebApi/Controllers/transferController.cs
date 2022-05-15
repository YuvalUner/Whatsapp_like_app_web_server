using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Data;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class transferController : ControllerBase {

        private readonly IContactsService _contactsService;

        public transferController(AdvancedProgrammingProjectsServerContext context) {
            this._contactsService = new DatabaseContactsService(context);
        }

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
