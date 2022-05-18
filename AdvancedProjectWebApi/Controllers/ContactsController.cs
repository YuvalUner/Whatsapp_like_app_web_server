#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain.DatabaseEntryModels;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Services.DataManipulation.Interfaces;

namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for managing everything related to a user's contacts.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[RequireHttps]
    public class ContactsController : ControllerBase {

        private readonly IContactsService _contactsService;
        private readonly IRegisteredUsersService _registeredUsersService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ContactsController(AdvancedProgrammingProjectsServerContext context) {
            _contactsService = new DatabaseContactsService(context);
            _registeredUsersService = new DatabaseRegisteredUsersService(context);
        }

        /// <summary>
        /// Gets all of a user's contacts.
        /// </summary>
        /// <returns>list of contacts or 404</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> getContacts() {
            string username = User.FindFirst("username")?.Value;

            List<Contact>? contacts = await _contactsService.GetContacts(username);
            if (contacts == null) {
                return NotFound();
            }
            return contacts;
        }

        /// <summary>
        /// Gets a specific contact for the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>contact if found, 404 if not found, 401 otherwise.</returns>
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

        /// <summary>
        /// Adds a new contact to the user.
        /// </summary>
        /// <param name="id">the contact's id</param>
        /// <param name="server">the contact's server</param>
        /// <param name="name">the contact's nickname</param>
        /// <returns>204 on success, 404 if contact not found, 401 otherwise.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(string id,[Bind("name,server")] Contact contact) {

            if (ModelState.IsValid) {
                string? user = User.FindFirst("username")?.Value;

                bool success = await _contactsService.editContact(user, contact.server, contact.name, id);

                if (success) {
                    return NoContent();
                }
                return NotFound();
            }
            return BadRequest();
        }

        /// <summary>
        /// Adds a user to the current user's contact list.
        /// </summary>
        /// <param name="contact">A contact with their id and server</param>
        /// <returns>201 on success, 404 if current user not found (should not happen), 401 otherwise.</returns>
        [HttpPost]
        public async Task<IActionResult> PostContact([Bind("id,server")] Contact contact, bool local = false) {

            if (ModelState.IsValid) {

                string user = User.FindFirst("username")?.Value;

                contact.last = null;
                contact.contactOf = user;
                contact.lastdate = DateTime.Now;
                if (local == true) {
                    contact.server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                }

                bool success = await _contactsService.addContact(user, contact);
                if (!success) {
                    return NotFound();
                }

                var invitiationContent = new {
                    from = user,
                    to = contact.id,
                    server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"
                };

                var invitation = JsonSerializer.Serialize(invitiationContent);

                var url = contact.server + "/api/invitations";

                using (var httpClient = new HttpClient()) {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.PostAsync(url, new StringContent(invitation, Encoding.UTF8, "application/json"));
                }

                return CreatedAtAction("PostContact", new { });
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes a user's contact
        /// </summary>
        /// <param name="id">the contact's id</param>
        /// <returns>204 on success, 404 if contact not found, 401 otherwise.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id) {

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

        /// <summary>
        /// Gets all the messages of a user with another user.
        /// </summary>
        /// <param name="id">the user's id</param>
        /// <returns>messages on success, 404 if other user not found, 401 otherwise.</returns>
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

        /// <summary>
        /// Adds a message to a conversation between 2 users.
        /// </summary>
        /// <param name="id">The other user's id</param>
        /// <param name="content">The message's content</param>
        /// <returns>204 on success, 404 if other user not found, or 401</returns>
        [HttpPost("{id}/messages")]
        public async Task<IActionResult> addMessage(string id, [Bind("content")] Message message) {

            // Not allowing empty messages
            if (id == null || message.content == null) {
                return BadRequest();
            }

            string currentUser = User.FindFirst("username")?.Value;
            message.created = DateTime.Now;
            message.type = "text";
            message.sent = true;

            bool success = await _contactsService.addMessage(currentUser, id, message);
            if (!success) {
                return NotFound();
            }

            // No risk of this being null if the previous function succeeded
            Contact? contact = await _contactsService.GetContact(currentUser, id);

            var transferContent = new {
                from = currentUser,
                to = id,
                content = message.content
            };

            var transfer = JsonSerializer.Serialize(transferContent);

            var url = contact.server + "/api/transfer";

            using (var httpClient = new HttpClient()) {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.PostAsync(url, new StringContent(transfer, Encoding.UTF8, "application/json")).Result;
            }

            return CreatedAtAction("addMessage", new { });
        }

        /// <summary>
        /// Gets a specific message from 2 user's conversation.
        /// </summary>
        /// <param name="id">The other user's id.</param>
        /// <param name="id2">The message's id</param>
        /// <returns>The message if found, 404 if not found, 401 otherwise.</returns>
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

        /// <summary>
        /// Edits a message in a conversation between 2 users.
        /// </summary>
        /// <param name="id">The other user's id</param>
        /// <param name="id2">The message's id</param>
        /// <param name="content">The new content</param>
        /// <returns>204 on success, 404 if message not found, 401 otherwise.</returns>
        [HttpPut("{id}/messsages/{id2}")]
        public async Task<IActionResult> EditMessage(string id, string id2, [Bind("content")] string content) {
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

        /// <summary>
        /// Deletes a specific message in a conversation.
        /// </summary>
        /// <param name="id">The other user's id</param>
        /// <param name="id2">The message's id</param>
        /// <returns>204 on success, 404 if message not found, 401 otherwise.</returns>
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

        /// <summary>
        /// Returns whether or not a user is already the current user's contact.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("alreadyContact/{user}")]
        public async Task<ActionResult<bool>> isAlreadyContact(string? user) {
            if (user == null) {
                return BadRequest();
            }
            string username = User.FindFirst("username")?.Value;
            bool result = await _contactsService.isAlreadyContact(username, user);
            return Ok(result);
        }

        /// <summary>
        /// Checks if a user is contact by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("byEmail/{email}")]
        public async Task<ActionResult<bool>> isAlreadyContactByEmail(string? email) {
            if (email == null) {
                return BadRequest();
            }
            string username = User.FindFirst("username")?.Value;
            bool result = await _contactsService.isAlreadyContactByEmail(username, email);
            return Ok(result);
        }

        /// <summary>
        /// Checks if a user is a contact by phone.
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("byPhone/{phone}")]
        public async Task<ActionResult<bool>> isAlreadyContactByPhone(string? phone) {
            if (phone == null) {
                return BadRequest();
            }
            string username = User.FindFirst("username")?.Value;
            bool result = await _contactsService.isAlreadyContactByPhone(username, phone);
            return Ok(result);
        }

        /// <summary>
        /// Adds a contact to a user by email.
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        [HttpPost("byEmail")]
        public async Task<IActionResult> addContactByEmail([Bind("id,server")] Contact contact, bool local = false) {
            if (ModelState.IsValid) {

                string user = User.FindFirst("username")?.Value;

                RegisteredUser rUser = await _registeredUsersService.GetRegisteredUserByEmail(contact.id);
                if (rUser == null) {
                    return NotFound();
                }

                contact.id = rUser.username;
                contact.last = null;
                contact.contactOf = user;
                contact.lastdate = DateTime.Now;
                if (local == true) {
                    contact.server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                }

                bool success = await _contactsService.addContact(user, contact);
                if (!success) {
                    return NotFound();
                }

                var invitiationContent = new {
                    from = user,
                    to = contact.id,
                    server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"
                };

                var invitation = JsonSerializer.Serialize(invitiationContent);

                var url = contact.server + "/api/invitations";

                using (var httpClient = new HttpClient()) {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.PostAsync(url, new StringContent(invitation, Encoding.UTF8, "application/json"));
                }

                return CreatedAtAction("PostContact", new { });
            }
            return BadRequest();
        }

        /// <summary>
        /// Adds a contact to a user by phone.
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="local"></param>
        /// <returns></returns>
        [HttpPost("byPhone")]
        public async Task<IActionResult> addContactByPhone([Bind("id,server")] Contact contact, bool local = false) {
            if (ModelState.IsValid) {

                string user = User.FindFirst("username")?.Value;

                RegisteredUser rUser = await _registeredUsersService.GetRegisteredUserByPhone(contact.id);
                if (rUser == null) {
                    return NotFound();
                }

                contact.id = rUser.username;
                contact.last = null;
                contact.contactOf = user;
                contact.lastdate = DateTime.Now;
                if (local == true) {
                    contact.server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                }

                bool success = await _contactsService.addContact(user, contact);
                if (!success) {
                    return NotFound();
                }

                var invitiationContent = new {
                    from = user,
                    to = contact.id,
                    server = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"
                };

                var invitation = JsonSerializer.Serialize(invitiationContent);

                var url = contact.server + "/api/invitations";

                using (var httpClient = new HttpClient()) {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await httpClient.PostAsync(url, new StringContent(invitation, Encoding.UTF8, "application/json"));
                }

                return CreatedAtAction("PostContact", new { });
            }
            return BadRequest();
        }
    }
}
