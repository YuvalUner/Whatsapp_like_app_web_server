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

namespace AdvancedProjectWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase {
        private readonly AdvancedProgrammingProjectsServerContext _context;

        public ContactsController() {
            _context = new AdvancedProgrammingProjectsServerContext();
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisteredUser>>> getContacts(string id) {
            RegisteredUser user = await _context.RegisteredUser.Where(ru => ru.username == id).Include(ru => ru.contacts).FirstOrDefaultAsync();
            List<RegisteredUser> contacts = new List<RegisteredUser>();
            foreach (Contact contact in user.contacts) {
                contacts.Add(await _context.RegisteredUser.Where(ru => ru.username == contact.name).FirstOrDefaultAsync());
            }
            return contacts;
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisteredUser>> GetRegisteredUser(string id, string currentUser) {
            RegisteredUser registeredUser = await _context.RegisteredUser.Where(ru => ru.username == currentUser).Include(ru => ru.contacts).FirstOrDefaultAsync();
            if (registeredUser == null) {
                return NotFound();
            }
            if (registeredUser.contacts.Find(c => c.name == id) != null) {
                return await _context.RegisteredUser.Where(ru => ru.username != currentUser).FirstOrDefaultAsync();
            }
            else {
                return NotFound();
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
        public async Task<ActionResult<RegisteredUser>> PostRegisteredUser(string user, string id) {
            RegisteredUser userToAdd = await _context.RegisteredUser.Where(ru => ru.username == user).Include(ru => ru.contacts)
                .Include(ru => ru.conversations).FirstOrDefaultAsync();
            if (userToAdd == null) {
                return NotFound();
            }
            RegisteredUser currentUser = await _context.RegisteredUser.Where(ru => ru.username == id).Include(ru => ru.contacts)
                .Include(ru => ru.conversations).FirstOrDefaultAsync();
            currentUser.contacts.Add(new Contact() { contactOf = id, name = user, LastSeen = DateTime.Now });
            currentUser.conversations.Add(new Conversation() { with = user, messages = new List<Message>() });
            userToAdd.contacts.Add(new Contact() { contactOf = user, name = id, LastSeen = DateTime.Now });
            userToAdd.conversations.Add(new Conversation() { with = user, messages = new List<Message>() });
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetRegisteredUser", new { id = userToAdd.username }, userToAdd);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredUser(string id, string username) {
            RegisteredUser firstUser = await _context.RegisteredUser.Where(ru => ru.username == username).Include(ru => ru.contacts).Include(ru => ru.conversations).FirstOrDefaultAsync();
            if (firstUser == null) {
                return NotFound();
            }
            Contact contactFirst = firstUser.contacts.Find(c => c.name == id);
            if (contactFirst != null) {
                Conversation convoOne = firstUser.conversations.Find(c => c.with == id);
                RegisteredUser secondUser = await _context.RegisteredUser.Where(ru => ru.username == id).Include(ru => ru.contacts).Include(ru => ru.conversations).FirstOrDefaultAsync();
                Contact contactSecond = secondUser.contacts.Find(c => c.name == username);
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
        public async Task<ActionResult<IEnumerable<Message>>> getMessages(string id, string username) {

            Contact con = await _context.Contact.Where(c => c.contactOf == username && c.name == id).FirstOrDefaultAsync();
            if (con != null) {
                Conversation convo = await _context.Conversation.Where(c => c.with == con.name).Include(c => c.messages).FirstOrDefaultAsync();
                return convo.messages;
            }
            else {
                return NotFound();
            }
        }

        private bool RegisteredUserExists(string id)
        {
            return _context.RegisteredUser.Any(e => e.username == id);
        }
    }
}
