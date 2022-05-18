using Microsoft.AspNetCore.SignalR;
using Domain.DatabaseEntryModels;
using Data;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;

namespace AdvancedProjectWebApi.Hubs {

    public class ChatAppHub: Hub {

        private readonly IRegisteredUsersService _registeredUsersService;
        private readonly IContactsService _contactsService;

        public ChatAppHub(AdvancedProgrammingProjectsServerContext context) {
            _contactsService = new DatabaseContactsService(context);
            _registeredUsersService = new DatabaseRegisteredUsersService(context);
        }

        /// <summary>
        /// On connection, user sends their username to map to a group.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task connected(string username) {

            await Groups.AddToGroupAsync(Context.ConnectionId, username);
        }

        /// <summary>
        /// Sends an event to the contact to update their chat
        /// </summary>
        /// <param name="convo"></param>
        /// <returns></returns>
        public async Task messageSent(string convo) {
            await Clients.Groups(convo).SendAsync("updateChat");
        }

        /// <summary>
        /// Sends an event to the contact to update their contact list.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task nicknameUpdated(string user) {
            RegisteredUser? rUser = await _contactsService.GetRegisteredUserWithConvoAndContacts(user);
            if (rUser != null) {
                foreach(Contact contact in rUser.contacts) {
                    await Clients.Group(contact.id).SendAsync("nicknameChanged");
                }
            }
        }

        /// <summary>
        /// Sends an event to the contact to update their contact list.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task addedContactByUsername(string username) {
            await Clients.Groups(username).SendAsync("updateContacts");
        }

        //Send update contacts to user.
        public async Task addedContactByPhone(string phone) {

            RegisteredUser? user = await _registeredUsersService.GetRegisteredUserByPhone(phone);
            if (user != null) {
                await Clients.Groups(user.username).SendAsync("updateContacts");
            }
        }

        /// <summary>
        /// Sends an event to the contact to update their contact list.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task addedContactByEmail(string email) {

            RegisteredUser? user = await _registeredUsersService.GetRegisteredUserByEmail(email);
            if (user != null) {
                await Clients.Groups(user.username).SendAsync("updateContacts");
            }
        }

        /// <summary>
        /// Sends an event to all the user's contacts to update their description of the contact.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task changedDescription(string user) {
            RegisteredUser? rUser = await _contactsService.GetRegisteredUserWithConvoAndContacts(user);
            if (rUser != null) {
                foreach (Contact contact in rUser.contacts) {
                    await Clients.Group(contact.id).SendAsync("descriptionChanged");
                }
            }
        }
    }
}
