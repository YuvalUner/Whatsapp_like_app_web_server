using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain.DatabaseEntryModels;
using Microsoft.EntityFrameworkCore;
using Services.DataManipulation.Interfaces;
using Domain.CodeOnlyModels;
using Utils;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class DatabaseRegisteredUsersService : IRegisteredUsersService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        public DatabaseRegisteredUsersService(AdvancedProgrammingProjectsServerContext context) {

            this._context = context;

        }

        public async Task<bool> doesUserExists(string? username) {
            if (username == null) {
                return false;
            }
            if (await this.GetRegisteredUser(username) == null) {
                return false;
            }
            return true;
        }

        public async Task<RegisteredUser?> GetRegisteredUser(string? username) {
            if (username == null) {
                return null;
            }
            RegisteredUser? user = await _context.RegisteredUser.Where(ru => ru.username == username).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool?> editDescription(string? username, string? newDescription) {
            if (username == null || newDescription == null) {
                return false;
            }
            RegisteredUser? user = await this.GetRegisteredUser(username);
            user.description = newDescription;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> getDescription(string? username) {
            if (username == null) {
                return null;
            }
            RegisteredUser? user = await this.GetRegisteredUser(username);
            if (user == null) {
                return null;
            }
            return user.description;
        }

        //public string? generateNickNum(){
        //    string nickNum = string.Empty;
        //    Random rand = new Random();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        int number = rand.Next(0, 10);
        //        nickNum = nickNum + number;
        //    }
        //    return nickNum;
        //}

        public async Task<string?> getNickNum(string? username) {
            if (username == null) {
                return null;
            }
            RegisteredUser? user = await this.GetRegisteredUser(username);
            if (user == null) {
                return null;
            }
            return user.nickname;
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

        public async Task<bool> editNickName(string? username, string? newNickName) {
            if (username == null || newNickName == null) {
                return false;
            }
            RegisteredUser? user = await this.GetRegisteredUser(username);
            if (user == null) {
                return false;
            }
            user.nickname = newNickName;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> verifyUser(string? username, string? password) {

            if (username == null || password == null) {
                return false;
            }
            RegisteredUser? user = await this.GetRegisteredUser(username);
            if (user == null) {
                return false;
            }
            // We used SHA256 for testing early on, before we knew more about handling passwords.
            // This made it a good opprotunity to show the reason why we save the algorithm used as well.
            if (user.hashingAlgorithm == "SHA256") {
                string hashedPassword = Utils.Utils.hashWithSHA256(password + user.salt);
                if (user.password == hashedPassword) {
                    user.salt = Utils.Utils.generateRandString(Utils.Utils.alphaNumericSpecial, Constants.saltLength);
                    user.password = Utils.Utils.HashWithPbkdf2(password, user.salt);
                    user.hashingAlgorithm = "Pbdkf2";
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            if (user.hashingAlgorithm == Constants.currentPasswordHash) {
                string hashedPassword = Utils.Utils.HashWithPbkdf2(password, user.salt);
                if (user.password == hashedPassword) {
                    return true;
                }
            }
            return false;

        }

        public async Task<bool> addNewRegisteredUser(PendingUser? pendingUser) {

            if (pendingUser != null) {
                RegisteredUser user = new RegisteredUser();
                user.username = pendingUser.username;
                user.password = pendingUser.password;
                user.phone = pendingUser.phone;
                user.email = pendingUser.email;
                user.hashingAlgorithm = pendingUser.hashingAlgorithm;
                user.salt = pendingUser.salt;
                user.nickname = pendingUser.nickname;
                user.verificationcode = null;
                user.secretQuestions = new SecretQuestion() {
                    Answer = pendingUser.secretQuestions.Answer,
                    Question = pendingUser.secretQuestions.Question
                };
                user.contacts = new List<Contact>();
                user.conversations = new List<Conversation>();
                user.nickNum = Utils.Utils.generateRandString(Utils.Utils.numeric, 4);
                user.verificationCodeCreationTime = new DateTime();
                _context.RegisteredUser.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
