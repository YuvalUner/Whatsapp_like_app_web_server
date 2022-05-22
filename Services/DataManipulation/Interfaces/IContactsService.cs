using Domain.DatabaseEntryModels;

namespace Services.DataManipulation.Interfaces {

    public interface IContactsService {

        /// <summary>
        /// Gets all contacts of a certain user
        /// </summary>
        /// <param name="username">The user to get contacts for</param>
        /// <returns>A list of all the user's contacts</returns>
        Task<List<Contact>?> GetContacts(string? username);

        /// <summary>
        /// Gets a specific contact from a specific user
        /// </summary>
        /// <param name="username">the specific user</param>
        /// <param name="contactToGet">the specific contact</param>
        /// <returns>The contact, or null if does not exist</returns>
        Task<Contact?> GetContact(string? username, string? contactToGet);

        /// <summary>
        /// Gets a user with their conversation and contacts
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<RegisteredUser?> GetRegisteredUserWithConvoAndContacts(string? username);

        /// <summary>
        /// Deletes a contact for a user
        /// </summary>
        /// <param name="user">the user to delete the contact from</param>
        /// <param name="contactToDelete">the contact to delete</param>
        /// <returns>True on success, False on failure</returns>
        Task<bool> DeleteContact(string? user, string? contactToDelete);

        /// <summary>
        /// Gets a specific conversation for a specific user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="with"></param>
        /// <returns></returns>
        Task<Conversation?> GetConversation(string? user, string? with);

        /// <summary>
        /// Adds a message to a conversation
        /// </summary>
        /// <param name="username">the user to add for</param>
        /// <param name="with">the user the conversation is with.</param>
        /// <param name="msg"></param>
        /// <returns>True on success, False on failure</returns>
        Task<bool> addMessage(string? username, string? with, Message msg);

        /// <summary>
        /// Adds a contact to a user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="contact"></param>
        /// <returns>True on success, False on failure</returns>
        Task<bool> addContact(string? username, Contact contact);

        /// <summary>
        /// Gets a specific message from a specific user's conversation.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="with"></param>
        /// <param name="msgId"></param>
        /// <returns>The message or null if not found</returns>
        public Task<Message?> GetMessage(string username, string with, int msgId);

        /// <summary>
        /// Edits a specific message.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="with"></param>
        /// <param name="msgId"></param>
        /// <returns>true on success, false otherwise.</returns>
        public Task<bool> editMessage(string username, string with, int msgId, string content);

        /// <summary>
        /// Deletes a specific message
        /// </summary>
        /// <param name="username"></param>
        /// <param name="with"></param>
        /// <param name="msgId"></param>
        /// <returns>true on success, false otherwise</returns>
        public Task<bool> DeleteMessage(string username, string with, int msgId);

        /// <summary>
        /// Edits a contact's info
        /// </summary>
        /// <param name="username"></param>
        /// <param name="server"></param>
        /// <param name="newName"></param>
        /// <param name="contactToGet"></param>
        /// <returns>true on success, false otherwise</returns>
        public Task<bool> editContact(string? username, string? server, string? newName, string? contactToGet);

        /// <summary>
        /// checks if user has contact already.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task<bool> isAlreadyContact(string? username, string? contact);

        /// <summary>
        /// gets contact's last seen.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task<string?> getLastSeen(string? username, string? contact);

        /// <summary>
        /// Gets a contact by their email.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<Contact> GetContactByEmail(string username, string? email);

        /// <summary>
        /// Returns whether a contact is a user's contact by their email.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<bool> isAlreadyContactByEmail(string username, string? email);

        /// <summary>
        /// Gets a contact by their phone number.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Task<Contact> GetContactByPhone(string username, string? phone);

        /// <summary>
        /// Returns whether a contact is a user's contact by their phone number.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<bool> isAlreadyContactByPhone(string username, string? email);
    }
}
