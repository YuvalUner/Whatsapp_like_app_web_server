using Microsoft.AspNetCore.Mvc;
using Domain.DatabaseEntryModels;
using Data;
using Services.DataManipulation.DatabaseContextBasedImplementations;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.SignalR;
using AdvancedProjectWebApi.Hubs;

namespace AdvancedProjectWebApi.Controllers {

    /// <summary>
    /// A controller for adding messages from other servers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // Ideally, should require this. But we don't know what the testers will be running.
    // [RequireHttps]
    public class transferController : ControllerBase {

        private readonly IContactsService _contactsService;
        private readonly IRegisteredUsersService _registeredUsersService;
        private readonly IConfiguration config;
        private readonly IHubContext<ChatAppHub> hub;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public transferController(IContactsService contactsService, IRegisteredUsersService registeredUsersService,
            IConfiguration config, IHubContext<ChatAppHub> hub) {
            this._contactsService = contactsService;
            this._registeredUsersService = registeredUsersService;
            this.config = config;
            this.hub = hub;
            if (FirebaseApp.DefaultInstance == null) {
                FirebaseApp.Create(new AppOptions() {
                    Credential = GoogleCredential.FromFile("private_key.json")
                });
            }
        }

        private async void sendFirebasePushNotification(RegisteredUser userFrom,
            RegisteredUser userTo, string content) {

            var message = new FirebaseAdmin.Messaging.Message() {
                Data = new Dictionary<string, string>() {
                    { "username", userFrom.username },
                    { "nickname", userFrom.nickname }
                },
                Notification = new Notification() {
                    Title = userFrom.nickname,
                    Body = content
                },
                Token = userTo.androidToken
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        /// <summary>
        /// Adds a message from another server.
        /// </summary>
        /// <param name="transfer">A message listing from who, to whom and its content</param>
        /// <returns>201 on success, 404 if user not found, 401 otherwise.</returns>
        [HttpPost]
        public async Task<IActionResult> transfer([Bind("from,to,content")] Transfer transfer) {

            if (ModelState.IsValid) {
                bool result = await _contactsService.addMessage(transfer.to, transfer.from,
                    new Domain.DatabaseEntryModels.Message {
                        content = transfer.content,
                        sent = false,
                        type = "text",
                        created = DateTime.Now
                    });
                if (result == true) {
                    RegisteredUser? userFrom = await _registeredUsersService.GetRegisteredUser(transfer.from);
                    RegisteredUser? userTo = await _registeredUsersService.GetRegisteredUser(transfer.to);
                    if (userFrom != null && userTo != null && userTo.androidToken != null) {
                        this.sendFirebasePushNotification(userFrom, userTo, transfer.content);
                    }
                    await hub.Clients.Groups(transfer.to).SendAsync("updateChat");
                    return CreatedAtAction("transfer", new { });
                }
                else {
                    return NotFound();
                }
            }
            return BadRequest();
        }
    }
}
