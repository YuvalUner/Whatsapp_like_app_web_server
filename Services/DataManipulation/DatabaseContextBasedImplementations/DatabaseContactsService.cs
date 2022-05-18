using Domain.DatabaseEntryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.DataManipulation.Interfaces;


namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class DatabaseContactsService : IContactsService {

        private readonly AdvancedProgrammingProjectsServerContext _context;
        private readonly IRegisteredUsersService _registeredUsersService;

        public DatabaseContactsService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
            this._registeredUsersService = new DatabaseRegisteredUsersService(context);
        }

        public async Task<bool> addContact(string? username, Contact contact) {
            RegisteredUser? user = await this.GetRegisteredUserWithConvoAndContacts(username);
            if (user == null) {
                return false;
            }

            contact.name = await _registeredUsersService.getNickname(contact.id);

            user.contacts.Add(contact);
            user.conversations.Add(new Conversation() { with = contact.id, messages = new List<Message>() });

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> addMessage(string? username, string? with, Message msg) {
            Conversation? convo = await this.GetConversation(username, with);

            if (convo == null) {
                return false;
            }

            convo.messages.Add(msg);

            _context.Entry(convo).State = EntityState.Modified;

            RegisteredUser? user = await this.GetRegisteredUserWithConvoAndContacts(with);
            if (user != null) {
                Contact? contact = user.contacts.Find(c => c.id == username);
                if (contact != null) {
                    contact.lastdate = DateTime.Now;
                    _context.Entry(contact).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteContact(string? user, string? contactToDelete) {
            if (user == null || contactToDelete == null) {
                return false;
            }

            RegisteredUser? currentUser = await this.GetRegisteredUserWithConvoAndContacts(user);
            if (currentUser == null) {
                return false;
            }

            // Users are not contacts of one another
            Contact? contact = currentUser.contacts.Find(c => c.id == contactToDelete);
            if (contact == null) {
                return false;
            }

            Conversation? convo = currentUser.conversations.Find(c => c.with == contactToDelete);
            if (convo == null) {
                return false;
            }

            _context.Conversation.Remove(convo);
            _context.Contact.Remove(contact);

            _context.Entry(currentUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Contact?> GetContact(string? username, string? contactToGet) {
            if (username == null || contactToGet == null) {
                return null;
            }

            Contact? contact = await _context.Contact.Where(c => c.contactOf == username && c.id == contactToGet).FirstOrDefaultAsync();

            return contact;
        }

        public async Task<List<Contact>?> GetContacts(string? username) {

            if (username == null) {
                return null;
            }

            List<Contact>? contacts = await _context.Contact.Where(c => c.contactOf == username).ToListAsync();
            return contacts;
        }

        public async Task<bool> isAlreadyContact(string? username, string? contact) {

            if (username == null || contact == null) {
                return false;
            }
            if (await GetContact(username, contact) == null) {
                return false;
            }
            return true;

        }

        public async Task<bool> editContact(string? username, string? server, string? newName, string? contactToGet) {
            Contact? contact = await this.GetContact(username, contactToGet);
            if (contact == null) {
                return false;
            }

            contact.name = newName;
            contact.server = server;

            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Conversation?> GetConversation(string? username, string? with) {
            if (username == null || with == null) {
                return null;
            }
            // An incredibly convoluted way to get the conversation from the database.
            // Done because all other ways resulted in exceptions due to multiple accesses to the context.
            // Therefore, single query was broken up into many inefficient ones with more information than needed.
            // Basically another example as to why bringing our JSON objects as models and letting 
            // Entity framework do everything was a terrible idea, but it is what it is.
            RegisteredUser? user = await this.GetRegisteredUserWithConvoAndContacts(username);
            if (user == null) {
                return null;
            }
            // If the users are not contacts, they have no conversation.
            if (user.contacts.Find(c => c.id == with) == null) {
                return null;
            }

            Conversation? convo = user.conversations.Find(c => c.with == with);

            convo = await _context.Conversation.Where(c => c.Id == convo.Id).Include(c => c.messages).FirstOrDefaultAsync();
            if (convo.messages == null) {
                convo.messages = new List<Message>();
            }
            return convo;
        }

        public async Task<RegisteredUser?> GetRegisteredUserWithConvoAndContacts(string? username) {
            if (username == null) {
                return null;
            }
            RegisteredUser? currentUser = await _context.RegisteredUser.Where(ru => ru.username == username).Include(ru => ru.contacts)
                                        .Include(ru => ru.conversations).FirstOrDefaultAsync();
            return currentUser;
        }

        public async Task<Message?> GetMessage(string username, string with, int msgId) {

            Conversation? convo = await this.GetConversation(username, with);
            if (convo == null) {
                return null;
            }
            Message? msg = convo.messages.Where(m => m.id == msgId).FirstOrDefault();
            if (msg != null) {
                return msg;
            }
            return null;
        }

        public async Task<bool> editMessage(string username, string with, int msgId, string content) {

            Message? msg = await this.GetMessage(username, with, msgId);
            if (msg != null) {
                msg.content = content;
                _context.Entry(msg).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMessage(string username, string with, int msgId) {

            Message? msg = await this.GetMessage(username, with, msgId);
            if (msg != null) {
                _context.Remove(msg);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string?> getLastSeen(string? username, string? contact) {
            if (username == null || contact == null) {
                return null;
            }
            Contact? contactSeeninfo = await this.GetContact(username, contact);
            if (contactSeeninfo == null) {
                return null;
            }
            return contactSeeninfo.last;
        }

        public async Task<Contact> GetContactByEmail(string username, string? email) {
            if (username == null || email == null) {
                return null;
            }
            RegisteredUser? rUser = await _registeredUsersService.GetRegisteredUserByEmail(email);
            if (rUser == null) {
                return null;
            }
            Contact? contact = await _context.Contact.Where(c => c.contactOf == username && c.id == rUser.username).FirstOrDefaultAsync();
            return contact;
        }

        public async Task<bool> isAlreadyContactByEmail(string username, string? email) {

            if (email == null) {
                return false;
            }
            if (await GetContactByEmail(username, email) != null) {
                return true;
            }
            return false;
        }

        public async Task<Contact> GetContactByPhone(string username, string? phone) {
            if (username == null || phone == null) {
                return null;
            }
            RegisteredUser? rUser = await _registeredUsersService.GetRegisteredUserByPhone(phone);
            if (rUser == null) {
                return null;
            }
            Contact? contact = await _context.Contact.Where(c => c.contactOf == username && c.id == rUser.username).FirstOrDefaultAsync();
            return contact;
        }

        public async Task<bool> isAlreadyContactByPhone(string username, string? email) {

            if (email == null) {
                return false;
            }
            if (await GetContactByPhone(username, email) != null) {
                return true;
            }
            return false;
        }
    }
}

