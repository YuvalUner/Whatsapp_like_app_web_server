using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;
using Domain.CodeOnlyModels;

namespace Services.DataManipulation.Interfaces {

    /// <summary>
    /// A service for handling registered users.
    /// </summary>
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
        /// Gets a registered user by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<RegisteredUser?> GetRegisteredUserByEmail(string? email);

        /// <summary>
        /// gets nickname of user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string?> getNickname(string? username);

        /// <summary>
        /// edit user's description.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newDescription"></param>
        /// <returns></returns>
        Task<bool?> editDescription(string? username, string? newDescription);

        /// <summary>
        /// get user description.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string?> getDescription(string? username);

        /// <summary>
        /// get user's nicknum.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string?> getNickNum(string? username);

        /// <summary>
        /// Adds a new registered user to the database.
        /// </summary>
        /// <param name="user">The pending user that finished their registration</param>
        /// <return>true on success, false otherwise</return>
        Task<bool> addNewRegisteredUser(PendingUser pendingUser);

        /// <summary>
        /// Checks if a user's username and password match.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if they do, false otherwise.</returns>
        Task<bool> verifyUser(string? username, string? password);

        /// <summary>
        /// updates user's password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> updatePassword(string? username, string? newPassword);

        /// <summary>
        /// edit's user nick name.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newNickName"></param>
        /// <returns></returns>
        Task<bool> editNickName(string? username, string? newNickName);

        /// <summary>
        /// veryfies user info.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> canVerify(string? username, string? input);


        /// <summary>
        /// veryfies matching email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> doEmailAndPasswordMAtch(string? email, string? password);

        /// <summary>
        /// veryfies secret question.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="questionNum"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        Task<bool> verifySecretQuestion(string? username, string? questionNum, string? answer);
    }
}
