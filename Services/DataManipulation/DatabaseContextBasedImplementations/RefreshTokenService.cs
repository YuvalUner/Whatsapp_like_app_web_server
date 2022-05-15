using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Services.TokenServices.Interfaces;
using Data;
using Domain.DatabaseEntryModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Services.TokenServices.Implementations;
using Services.DataManipulation.Interfaces;

namespace Services.DataManipulation.DatabaseContextBasedImplementations {

    public class RefreshTokenService : IRefreshTokenService {

        private readonly AdvancedProgrammingProjectsServerContext _context;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;

        public RefreshTokenService(AdvancedProgrammingProjectsServerContext context) {
            this._context = context;
            this.refreshTokenGenerator = new AuthTokenGenerator();
        }

        public async Task<bool> storeRefreshToken(RefreshToken token) {
            RefreshToken hashedToken = new RefreshToken() {
                Token = Utils.Utils.hashWithSHA256(token.Token),
                RegisteredUserusername = token.RegisteredUserusername,
                ExpiryDate = token.ExpiryDate
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

        public async Task<RefreshToken> renewRefreshToken(RefreshToken token) {

            await this.RemoveToken(token);
            RefreshToken newToken = new RefreshToken() {
                RegisteredUserusername = token.RegisteredUserusername,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                Token = this.refreshTokenGenerator.GenerateRefreshToken()
            };
            await this.storeRefreshToken(newToken);
            return newToken;
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
    }
}
