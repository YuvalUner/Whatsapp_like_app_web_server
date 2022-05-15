using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;
using Domain.CodeOnlyModels;
using Data;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Utils;
using Services.DataManipulation.Interfaces;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class DatabasePendingUsersService : IPendingUsersService {

        private readonly AdvancedProgrammingProjectsServerContext _context;
        private static readonly int codeLengths = 6;

        public DatabasePendingUsersService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
        }

        public async Task<PendingUser?> GetPendingUser(string? username) {
            if (username == null) {
                return null;
            }
            PendingUser? user = await _context.PendingUser.Where(pu => pu.username == username).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> doesUserExist(string? username) {
            if (username == null) {
                return false;
            }
            PendingUser? user = await this.GetPendingUser(username);
            if (user != null) {
                return true;
            }
            return false;
        }

        public async Task<bool> RenewCode(PendingUser? user, MailRequest mail) {

            if (user != null) {
                user.verificationCode = Utils.Utils.generateRandString(Utils.Utils.alphaNumeric, codeLengths);
                user.verificationCodeCreationTime = DateTime.UtcNow;
                _context.Entry(user).State = EntityState.Modified;
                mail.Body = ($"<p>Your verification code is:</p><h3>{user.verificationCode}</h3>" +
                    $"<p>It will be valid for the next 30 minutes</p>");
                mail.ToEmail = user.email;
                Utils.Utils.sendEmail(mail);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> addToPending(PendingUser pendingUser, string hasingAlgorithm, MailRequest mail) {

            pendingUser.timeCreated = DateTime.UtcNow;
            pendingUser.salt = Utils.Utils.generateRandString(Utils.Utils.alphaNumericSpecial, codeLengths);

            pendingUser.hashingAlgorithm = hasingAlgorithm;
            if (hasingAlgorithm == "SHA256") {
                pendingUser.password = Utils.Utils.hashWithSHA256(pendingUser.password + pendingUser.salt);
            }
            pendingUser.verificationCode = Utils.Utils.generateRandString(Utils.Utils.alphaNumeric, codeLengths);
            mail.Body = ($"<p>Your verification code is:</p><h3>{pendingUser.verificationCode}</h3>" +
                $"<p>It will be valid for the next 30 minutes</p>");
            Utils.Utils.sendEmail(mail);
            pendingUser.verificationCodeCreationTime = DateTime.UtcNow;
            _context.PendingUser.Add(pendingUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> canVerify(PendingUser? user, string? code) {

            if (user == null) {
                return false;
            }
            if (user.verificationCode == code
                && DateTime.UtcNow.Subtract(user.verificationCodeCreationTime).TotalMinutes <= 30) {
                return true;
            }
            return false;
        }

        public async Task<bool> RemovePendingUser(PendingUser? user) {

            if (user != null) {
                _context.PendingUser.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<PendingUser?> GetPendingUserWithSecretQuestion(string? username) {
            if (username == null) {
                return null;
            }
            PendingUser? user = await _context.PendingUser.Where(pu => pu.username == username).Include(pu => pu.secretQuestions).FirstOrDefaultAsync();
            return user;
        }
    }
}
