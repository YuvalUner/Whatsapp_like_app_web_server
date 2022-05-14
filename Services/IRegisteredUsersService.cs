using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services {

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
    }
}
