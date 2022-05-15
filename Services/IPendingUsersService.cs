using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services {

    public interface IPendingUsersService {

        /// <summary>
        /// Adds a pending user to the pending users table
        /// </summary>
        /// <param name="pendingUser"></param>
        /// <returns></returns>
        Task<bool> addToPending(PendingUser pendingUser, string encryptionAlgorithm, MailRequest mail);

        /// <summary>
        /// Checks if a pending user exists in the database or not.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True if it exists, false otherwise.</returns>
        Task<bool> doesUserExist(string? username);

        /// <summary>
        /// Gets a registered user from the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The user if it exists, null otherwise.</returns>
        public Task<PendingUser?> GetPendingUser(string? username);

        /// <summary>
        /// Renews the user's verification code.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<bool> RenewCode(PendingUser? user, MailRequest mail);
    }
}
