using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;
using Domain.CodeOnlyModels;

namespace Services.DataManipulation.Interfaces {

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

        /// <summary>
        /// Checks whether the code the user input in the sign up process matches their code.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <returns>True if they match, false otherwise</returns>
        public Task<bool> canVerify(PendingUser? user, string? code);

        /// <summary>
        /// Removes a pending user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true on success, false otherwise.</returns>
        public Task<bool> RemovePendingUser(PendingUser? user);

        /// <summary>
        /// Gets the pending user with their secret question.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Task<PendingUser?> GetPendingUserWithSecretQuestion(string? username);

        /// <summary>
        /// get pending user by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<PendingUser?> GetPendingUserByEmail(string? email);

        /// <summary>
        /// checks if pending user exists by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> doesPendingUserExistsByEmail(string? email);

        /// <summary>
        /// checks if pending user exists by phone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<bool> doesPendingUserExistsByPhone(string? phone);

        /// <summary>
        /// get pending user by their phone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        Task<PendingUser?> GetPendingUserByPhone(string? phone);
    }
}
