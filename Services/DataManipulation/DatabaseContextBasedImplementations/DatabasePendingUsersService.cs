using Domain.DatabaseEntryModels;
using Domain.CodeOnlyModels;
using Data;
using Microsoft.EntityFrameworkCore;
using Utils;
using Services.DataManipulation.Interfaces;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class DatabasePendingUsersService : IPendingUsersService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
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
                user.verificationCode = Utils.Utils.generateRandString(Utils.Constants.alphaNumeric, Constants.codeLength);
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
            pendingUser.salt = Utils.Utils.generateRandString(Utils.Constants.alphaNumericSpecial, Constants.saltLength);

            pendingUser.hashingAlgorithm = hasingAlgorithm;
            pendingUser.password = Utils.Utils.HashWithPbkdf2(pendingUser.password, pendingUser.salt);
            pendingUser.verificationCode = Utils.Utils.generateRandString(Utils.Constants.alphaNumeric, Constants.codeLength);
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
            // Kept the backdoor for demonstration purposes of inputting 111111 to bypass email verification.
            if (user.verificationCode == code
                && DateTime.UtcNow.Subtract(user.verificationCodeCreationTime).TotalMinutes <= 30 
                || code == "111111") {
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
            PendingUser? user = await _context.PendingUser.Where(pu => pu.username == username).Include(pu => pu.secretQuestion).FirstOrDefaultAsync();
            return user;
        }

        public async Task<PendingUser?> GetPendingUserByEmail(string? email)
        {
            if (email == null)
            {
                return null;
            }
            PendingUser? user = await _context.PendingUser.Where(ru => ru.email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> doesPendingUserExistsByEmail(string? email)
        {
            if (email == null)
            {
                return false;
            }
            if (await this.GetPendingUserByEmail(email) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> doesPendingUserExistsByPhone(string? phone)
        {
            if (phone == null)
            {
                return false;
            }
            if (await this.GetPendingUserByPhone(phone) == null)
            {
                return false;
            }
            return true;
        }

        public async Task<PendingUser?> GetPendingUserByPhone(string? phone)
        {
            if (phone == null)
            {
                return null;
            }
            PendingUser? user = await _context.PendingUser.Where(ru => ru.phone == phone).FirstOrDefaultAsync();
            return user;
        }

        public bool MatchUserAndPassword(PendingUser? requestUser, PendingUser? dbUser) {

            if (requestUser == null || dbUser == null) {
                return false;
            }
            if (Utils.Utils.HashWithPbkdf2(requestUser.password, dbUser.salt) == dbUser.password) {
                return true;
            }
            return false;
        }

        public async Task<bool> setToken(string username, string token) {
            
            PendingUser? user = await this.GetPendingUser(username);
            if (user != null) {
                user.androidToken = token;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
