using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;
using Domain.CodeOnlyModels;

namespace Services.DataManipulation.Interfaces {

    public interface IRegisteredUsersService {

        /// <summary>
        /// Returns whether a user with a specified username exists or not.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> doesUserExists(string? username);

        /// <summary>
        /// Gets a registered user by their username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Registered user or null if they do not exist</returns>
        Task<RegisteredUser?> GetRegisteredUser(string? username);

        /// <summary>
        /// gets nickname of user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string?> getNickname(string? username);

        /// <summary>
        /// Adds a new registered user to the database.
        /// </summary>
        /// <param name="user">The pending user that finished their registration</param>
        /// <return>true on success, false otherwise</return>
        Task<bool> addNewRegisteredUser(PendingUser pendingUser);
    }
}
