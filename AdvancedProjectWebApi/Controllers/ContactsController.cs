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

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase {
        private readonly AdvancedProgrammingProjectsServerContext _context;
        private readonly IContactsService _contactsService;

        public ContactsController(AdvancedProgrammingProjectsServerContext context) {
            _context = context;
            _contactsService = new DatabaseContactsService(context);
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> getContacts(string id) {
            List<Contact>? contacts = await _contactsService.GetContacts(id);
            if (contacts == null) {
                return NotFound();
            }
            return contacts;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> getContact(string id, string currentUser) {
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
        public async Task<IActionResult> PutRegisteredUser(string id, RegisteredUser registeredUser) {
            if (id != registeredUser.username) {
                return BadRequest();
            }

            _context.Entry(registeredUser).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!RegisteredUserExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostContact(string user, string id, string server) {

            await _contactsService.addContact(user, new Contact() {
                contactOf = user,
                id = id,
                last = null,
                server = server,
                lastdate = DateTime.Now
            });

            await _contactsService.addContact(id, new Contact() {
                contactOf = id,
                id = user,
                last = null,
                server = server,
                lastdate = DateTime.Now
            });

            return CreatedAtAction("PostContact", new { id = id });
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredUser(string id, string username) {

            await _contactsService.DeleteContact(username, id);
            await _contactsService.DeleteContact(id, username);

            return NoContent();
        }

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> getMessages(string id, string currentUser) {

            Conversation convo = await _contactsService.GetConversation(currentUser, id);

            return convo.messages;
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> addMessage(string currentUser, string id, string content) {

            await _contactsService.addMessage(currentUser, id, new Message() { content = content, created = DateTime.Now, type = "text", sent = true });
            await _contactsService.addMessage(id, currentUser, new Message() { content = content, created = DateTime.Now, type = "text", sent = false });

            return CreatedAtAction("addMessage", new { content = content });
        }

        private bool RegisteredUserExists(string id) {
            return _context.RegisteredUser.Any(e => e.username == id);
        }
    }
}
