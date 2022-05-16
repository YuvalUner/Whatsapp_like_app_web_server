using Data;
using Domain.DatabaseEntryModels;
using Microsoft.EntityFrameworkCore;
using Services.DataManipulation.Interfaces;
using Services.TokenServices.Implementations;
using Services.TokenServices.Interfaces;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class RefreshTokenService : IRefreshTokenService {

        private readonly AdvancedProgrammingProjectsServerContext _context;

        public RefreshTokenService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
        }

        public async Task<bool> storeRefreshToken(string token, string username, string userAgent) {
            RefreshToken hashedToken = new RefreshToken() {
                Token = Utils.Utils.hashWithSHA256(token),
                RegisteredUserusername = username,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                UserAgent = userAgent
            };
            _context.RefreshToken.Add(hashedToken);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RefreshToken?> GetToken(string? token) {

            if (token == null) {
                return null;
            }
            string hashedToken = Utils.Utils.hashWithSHA256(token);
            RefreshToken? rToken = await _context.RefreshToken.Where(r => r.Token == hashedToken).FirstOrDefaultAsync();
            return rToken;
        }

        public async Task<bool> RemoveToken(RefreshToken token) {
            _context.RefreshToken.Remove(token);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> validateTokenExpiry(RefreshToken? token) {
            if (token == null) {
                return false;
            }
            // Any token that did not pass its expiry date should cause this to be a negative value;
            if (DateTime.UtcNow.Subtract(token.ExpiryDate).TotalMinutes < 0) {
                return true;
            }
            await this.RemoveToken(token);
            return false;
        }

        public async Task<bool> RemovePreviousTokens(string? username, string? userAgent) {

            if (username == null) {
                return false;
            }

            List<RefreshToken> prevTokens = await _context.RefreshToken.Where(r => r.RegisteredUserusername == username && r.UserAgent == userAgent).ToListAsync();
            foreach(RefreshToken token in prevTokens) {
                _context.RefreshToken.Remove(token);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
