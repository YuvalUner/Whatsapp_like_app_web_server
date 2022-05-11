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

namespace AdvancedProjectWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase {
        private readonly AdvancedProgrammingProjectsServerContext _context;

        public ContactsController(AdvancedProgrammingProjectsServerContext context) {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> getContacts(string id) {
            List<Contact> contacts = await _context.Contact.Where(c => c.contactOf == id).ToListAsync();
            return contacts;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> getContact(string id, string currentUser) {
            Contact contact = await _context.Contact.Where(c => c.contactOf == currentUser && c.id == id).FirstOrDefaultAsync();
            // Contact contact = await _context.Contact.Where(c => c.contactOf == currentUser && c.id == id).FirstOrDefaultAsync();
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
            // Get the contact to be added.
            RegisteredUser userToAdd = await _context.RegisteredUser.Where(ru => ru.username == user).Include(ru => ru.contacts)
                .Include(ru => ru.conversations).FirstOrDefaultAsync();
            // Make sure it exists
            if (userToAdd == null) {
                return NotFound();
            }
            // Mutually add both users to each other's contacts
            RegisteredUser currentUser = await _context.RegisteredUser.Where(ru => ru.username == id).Include(ru => ru.contacts)
                .Include(ru => ru.conversations).FirstOrDefaultAsync();
            currentUser.contacts.Add(new Contact() {
                contactOf = id,
                id = user,
                last = null,
                name = userToAdd.nickname,
                server = server,
                lastdate = DateTime.Now
            });
            currentUser.conversations.Add(new Conversation() { with = user, messages = new List<Message>() });
            userToAdd.contacts.Add(new Contact() {
                contactOf = user,
                id = id,
                name = currentUser.nickname,
                last = null,
                server = server,
                lastdate = DateTime.Now
            });
            userToAdd.conversations.Add(new Conversation() { with = id, messages = new List<Message>() });
            await _context.SaveChangesAsync();

            Response.StatusCode = 201;

            return CreatedAtAction("PostContact", new { id = id });
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredUser(string id, string username) {
            // Check if the user itself exists
            RegisteredUser firstUser = await _context.RegisteredUser.Where(ru => ru.username == username).Include(ru => ru.contacts).Include(ru => ru.conversations).FirstOrDefaultAsync();
            if (firstUser == null) {
                return NotFound();
            }
            // check if the other user is in the user's contact list
            Contact contactFirst = firstUser.contacts.Find(c => c.id == id);
            if (contactFirst != null) {
                // If yes, mutually delete them from each other's contacts and conversations.
                Conversation convoOne = firstUser.conversations.Find(c => c.with == id);
                RegisteredUser secondUser = await _context.RegisteredUser.Where(ru => ru.username == id).Include(ru => ru.contacts).Include(ru => ru.conversations).FirstOrDefaultAsync();
                Contact contactSecond = secondUser.contacts.Find(c => c.id == username);
                Conversation convoTwo = secondUser.conversations.Find(c => c.with == username);
                _context.Conversation.Remove(convoOne);
                _context.Conversation.Remove(convoTwo);
                _context.Contact.Remove(contactFirst);
                _context.Contact.Remove(contactSecond);
                await _context.SaveChangesAsync();
            }
            else {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/messages")]
        public async Task<ActionResult<IEnumerable<Message>>> getMessages(string id, string currentUser) {

            // Getting the conversation between both users and the messages in that conversation.
            Conversation convo = (from conversation in
                                            (await (from user in _context.RegisteredUser
                                                    where user.username == currentUser
                                                    select user.conversations).FirstOrDefaultAsync())
                                          join message in _context.Message on conversation.Id equals message.ConversationId
                                          where conversation.with == id
                                          select conversation).FirstOrDefault();

            return convo.messages;
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> addMessage(string currentUser, string id, string content) {

            // Getting the conversations of both users and the messages in those, so they can be added to.
            Conversation convoSender = (from conversation in
                                        (await (from user in _context.RegisteredUser
                                         where user.username == currentUser
                                        select user.conversations).FirstOrDefaultAsync())
                                        join message in _context.Message on conversation.Id equals message.ConversationId
                                        where conversation.with == id
                                        select conversation).FirstOrDefault();

            Conversation convoReceiver = (from conversation in
                                            (await (from user in _context.RegisteredUser
                                         where user.username == id
                                         select user.conversations).FirstOrDefaultAsync())
                                          join message in _context.Message on conversation.Id equals message.ConversationId
                                          where conversation.with == currentUser
                                          select conversation).FirstOrDefault();

            // Adding the message and crying over data redundancy
            convoSender.messages.Add(new Message() { content = content, created = DateTime.Now, type = "text", sent = true });
            convoReceiver.messages.Add(new Message() { content = content, created = DateTime.Now, type = "text", sent = false });

            _context.Entry(convoSender).State = EntityState.Modified;
            _context.Entry(convoReceiver).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return CreatedAtAction("addMessage", new { content = content });
        }

        private bool RegisteredUserExists(string id) {
            return _context.RegisteredUser.Any(e => e.username == id);
        }
    }
}
