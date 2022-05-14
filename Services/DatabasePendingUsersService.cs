using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Data;
using System.Security.Cryptography;

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

        public async Task<bool> addToPending(PendingUser pendingUser, string encryptionAlgorithm) {

            pendingUser.timeCreated = DateTime.Now;
            pendingUser.salt = this.generateSalt();
            pendingUser.encryptionAlgorithm = encryptionAlgorithm;
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] vs = sha256.ComputeHash(new UTF8Encoding().GetBytes(pendingUser.password));
            }
            var a = 5;
            return true;
        }
    }
}
