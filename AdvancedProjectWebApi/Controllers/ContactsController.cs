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
    public class ContactsController : ControllerBase
    {
        private readonly AdvancedProgrammingProjectsServerContext _context;

        public ContactsController()
        {
            _context = new AdvancedProgrammingProjectsServerContext();
        }

        // GET: api/RegisteredUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetRegisteredUser(string id)
        {
            return await _context.Contact.Where(c => c.contactOf == id).ToListAsync();
        }

        // GET: api/RegisteredUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisteredUser>> GetRegisteredUser(string id, string currentUser)
        {
            var registeredUser = await _context.RegisteredUser.FindAsync(id);

            if (registeredUser == null)
            {
                return NotFound();
            }

            return registeredUser;
        }

        // PUT: api/RegisteredUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegisteredUser(string id, RegisteredUser registeredUser)
        {
            if (id != registeredUser.username)
            {
                return BadRequest();
            }

            _context.Entry(registeredUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisteredUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RegisteredUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RegisteredUser>> PostRegisteredUser(string user, string id)
        {
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

        // DELETE: api/RegisteredUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredUser(string id)
        {
            var registeredUser = await _context.RegisteredUser.FindAsync(id);
            if (registeredUser == null)
            {
                return NotFound();
            }

            _context.RegisteredUser.Remove(registeredUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegisteredUserExists(string id)
        {
            return _context.RegisteredUser.Any(e => e.username == id);
        }
    }
}
