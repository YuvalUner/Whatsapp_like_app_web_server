using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services {

    public class DatabaseContactsService : IContactsService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        public DatabaseContactsService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
        }

        public async Task<string?> getNickname(string? username) {
            if (username == null) {
                return null;
            }
            string? nickname = await (from user in _context.RegisteredUser
                              where user.username == username
                              select user.nickname).FirstOrDefaultAsync();
            return nickname;
        }

        public async Task<bool> addContact(string? username, Contact contact) {
            RegisteredUser? user = await this.GetRegisteredUserWithConvoAndContacts(username);
            if (user == null) {
                return false;
            }

            contact.name = await this.getNickname(contact.id);

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

        public async Task<List<Contact>?> GetContacts(string username) {

            if (username == null) {
                return null;
            }

            List<Contact>? contacts = await _context.Contact.Where(c => c.contactOf == username).ToListAsync();
            return contacts;
        }

        public async Task<Conversation?> GetConversation(string? username, string? with) {
            if (username == null || with == null) {
                return null;
            }
            Conversation? convo = (from conversation in
                                    (await (from user in _context.RegisteredUser
                                            where user.username == username
                                            select user.conversations).FirstOrDefaultAsync())
                                  join message in _context.Message on conversation.Id equals message.ConversationId
                                  where conversation.with == with
                                  select conversation).FirstOrDefault();
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
    }
}
