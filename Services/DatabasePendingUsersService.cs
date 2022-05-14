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

        public async Task<bool> addToPending(PendingUser pendingUser, string encryptionAlgorithm, MailRequest mail) {

            pendingUser.timeCreated = DateTime.Now;
            pendingUser.salt = Utils.generateRandString(saltString, 6);
            pendingUser.encryptionAlgorithm = encryptionAlgorithm;
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] result = sha256.ComputeHash(new UTF8Encoding().GetBytes(pendingUser.password + pendingUser.salt));
                pendingUser.password = Encoding.UTF8.GetString(result, 0, result.Length);
            }
            pendingUser.verificationcode = Utils.generateRandString(verCodeString, 6);
            mail.Body = ($"<p>Your verification code is:</p><br><h3>{pendingUser.verificationcode}</h3><br>" +
                $"<p>It will be valid for the next 30 minutes</p>");
            Utils.sendEmail(mail);
            var a = 5;
            _context.PendingUser.Add(pendingUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
