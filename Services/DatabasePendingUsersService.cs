using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Data;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Services {
    public class DatabasePendingUsersService : IPendingUsersService {

        private readonly AdvancedProgrammingProjectsServerContext _context;
        private static readonly string saltString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+~?'][<>.,";
        private static readonly string verCodeString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
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
                user.verificationCode = Utils.generateRandString(verCodeString, codeLengths);
                user.verificationCodeCreationTime = DateTime.UtcNow;
                _context.Entry(user).State = EntityState.Modified;
                mail.Body = ($"<p>Your verification code is:</p><h3>{user.verificationCode}</h3>" +
                    $"<p>It will be valid for the next 30 minutes</p>");
                mail.ToEmail = user.email;
                Utils.sendEmail(mail);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> addToPending(PendingUser pendingUser, string hasingAlgorithm, MailRequest mail) {

            pendingUser.timeCreated = DateTime.UtcNow;
            pendingUser.salt = Utils.generateRandString(saltString, codeLengths);

            pendingUser.hashingAlgorithm = hasingAlgorithm;
            if (hasingAlgorithm == "SHA256") {
                using (SHA256 sha256 = SHA256.Create()) {
                    byte[] result = sha256.ComputeHash(new UTF8Encoding().GetBytes(pendingUser.password + pendingUser.salt));
                    pendingUser.password = Encoding.UTF8.GetString(result, 0, result.Length);
                }
            }
            pendingUser.verificationCode = Utils.generateRandString(verCodeString, codeLengths);
            mail.Body = ($"<p>Your verification code is:</p><h3>{pendingUser.verificationCode}</h3>" +
                $"<p>It will be valid for the next 30 minutes</p>");
            Utils.sendEmail(mail);
            pendingUser.verificationCodeCreationTime = DateTime.UtcNow;
            _context.PendingUser.Add(pendingUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
