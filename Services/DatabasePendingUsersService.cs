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

        public DatabasePendingUsersService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
        }

        private string generateSalt() {
            string salt = "";
            string selectionString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+~?'][<>.,";
            int len = selectionString.Length;
            Random rand = new Random();
            for (int i = 0; i < 6; i++) {
                salt += selectionString[rand.Next(len)];
            }
            return salt;
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

        public async Task<bool> addToPending(PendingUser pendingUser, string encryptionAlgorithm) {

            pendingUser.timeCreated = DateTime.Now;
            pendingUser.salt = this.generateSalt();
            pendingUser.encryptionAlgorithm = encryptionAlgorithm;
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] result = sha256.ComputeHash(new UTF8Encoding().GetBytes(pendingUser.password + pendingUser.salt));
                pendingUser.password = System.Text.Encoding.UTF8.GetString(result, 0, result.Length);
            }
            var a = 5;
            _context.PendingUser.Add(pendingUser);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
