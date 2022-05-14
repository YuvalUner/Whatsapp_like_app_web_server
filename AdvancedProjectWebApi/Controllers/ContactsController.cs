#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequireHttps]
    public class ContactsController : ControllerBase {
        private readonly AdvancedProgrammingProjectsServerContext _context;
        private readonly IContactsService _contactsService;

        public ContactsController(AdvancedProgrammingProjectsServerContext context) {
            _context = context;
            _contactsService = new DatabaseContactsService(context);
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> getContacts() {
            string username = User.FindFirst("username")?.Value;

            List<Contact>? contacts = await _contactsService.GetContacts(username);
            if (contacts == null) {
                return NotFound();
            }
            return contacts;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> getContact(string id) {
            if (id == null) {
                return BadRequest();
            }
            string currentUser = User.FindFirst("username")?.Value;

            Contact contact = await _contactsService.GetContact(currentUser, id);
            if (contact == null) {
                return NotFound();
            }
            else {
                return contact;
            }
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(string id, string server, string name) {

            string? user = User.FindFirst("username")?.Value;
            if (user == null || id == null || name == null || server == null)
            {
                return BadRequest();
            }

            bool success = await _contactsService.editContact(user, server, name, id);

            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostContact(string id, string server) {

            if (id == null || server == null) {
                return BadRequest();
            }

            string user = User.FindFirst("username")?.Value;

            bool success = await _contactsService.addContact(user, new Contact() {
                contactOf = user,
                id = id,
                last = null,
                server = server,
                lastdate = DateTime.Now
            });
            if (!success) {
                return NotFound();
            }

            var invitiationContent = new {
                from = user,
                to = id,
                server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"
            };

            var invitation = JsonSerializer.Serialize(invitiationContent);

            var url = server + "/api/invitations";

            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(url, new StringContent(invitation, Encoding.UTF8, "application/json"));
            }

            return CreatedAtAction("PostContact", new {});
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredUser(string id) {

            if (id == null) {
                return BadRequest();
            }

            string username = User.FindFirst("username")?.Value;

            bool success = await _contactsService.DeleteContact(username, id);
            if (!success) {
                return NotFound();
            }
            success = await _contactsService.DeleteContact(id, username);
            if (!success) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> getMessages(string id) {

            if (id == null) {
                return BadRequest();
            }

            string currentUser = User.FindFirst("username")?.Value;

            Conversation? convo = await _contactsService.GetConversation(currentUser, id);

            if (convo == null) {
                return NotFound();
            }

            return convo.messages;
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> addMessage(string id, string content) {

            // Not allowing empty messages
            if (id == null || content == null) {
                return BadRequest();
            }

            string currentUser = User.FindFirst("username")?.Value;

            bool success = await _contactsService.addMessage(currentUser, id, new Message() { content = content, created = DateTime.Now, type = "text", sent = true });
            if (!success) {
                return NotFound();
            }

            // No risk of this being null if the previous function succeeded
            Contact? contact = await _contactsService.GetContact(currentUser, id);

            var transferContent = new {
                from = currentUser,
                to = id,
                content = content
            };

            var transfer = JsonSerializer.Serialize(transferContent);

            var url = contact.server + "/api/transfer";

            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.PostAsync(url, new StringContent(transfer, Encoding.UTF8, "application/json")).Result;
            }

            return CreatedAtAction("addMessage", new {});
        }

        [HttpGet("{id}/messages/{id2}")]
        public async Task<ActionResult<Message>> getMessage(string id, string id2) {

            if (id == null || id2 == null) {
                return BadRequest();
            }

            int id2AsInt;
            try {
                id2AsInt = Int32.Parse(id2);
            }
            catch {
                return BadRequest();
            }

            string currentUser = User.FindFirst("username")?.Value;


            Message msg = await _contactsService.GetMessage(currentUser, id, id2AsInt);
            if (msg != null) {
                return msg;
            }
            return NotFound();
        }

        [HttpPut("{id}/messsages/{id2}")]
        public async Task<IActionResult> EditMessage(string id, string id2, string content) {
            if (id == null || id2 == null) {
                return BadRequest();
            }

            int id2AsInt;
            try {
                id2AsInt = Int32.Parse(id2);
            }
            catch {
                return BadRequest();
            }


            string currentUser = User.FindFirst("username")?.Value;

            bool success = await _contactsService.editMessage(currentUser, id, id2AsInt, content);
            if (success) {
                return NoContent();
            }
            return NotFound();

        }

        [HttpDelete("{id}/messages/{id2}")]
        public async Task<IActionResult> DeleteMessage(string id, string id2) {

            if (id == null || id2 == null) {
                return BadRequest();
            }

            int id2AsInt;
            try {
                id2AsInt = Int32.Parse(id2);
            }
            catch {
                return BadRequest();
            }

            string currentUser = User.FindFirst("username")?.Value;
            bool success = await _contactsService.DeleteMessage(currentUser, id, id2AsInt);
            if (success) {
                return NoContent();
            }
            return NotFound();
        }

        private bool RegisteredUserExists(string id) {
            return _context.RegisteredUser.Any(e => e.username == id);
        }
    }
}
