﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.CodeOnlyModels;
using Microsoft.IdentityModel.Tokens;
using Services.TokenServices.Interfaces;
using Utils;

namespace Services.TokenServices.Implementations {

    /// <summary>
    /// A generator for auth tokens.
    /// Can be treated as a generator for each of the individual types, or as all of them together, depending
    /// on the interface chosen as the static type on creation.
    /// </summary>
    public class AuthTokenGenerator : IAuthTokenGenerator, IRefreshTokenGenerator, IAccessTokenGenerator, IHybridTokenGenerator {

        public string GenerateAccessToken(string username, string subject, string key, string issuer, string audience, int expiry = 5) {
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("username", username)
                };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                //expires: DateTime.UtcNow.AddMinutes(expiry),
                // TODO: Remove this line and uncomment previous line when not testing in swagger.
                expires: DateTime.UtcNow.AddMinutes(Constants.accessTokenTestingExpiry),
                signingCredentials: mac
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AuthToken GenerateAuthToken(string username, string subject, string key, string issuer, string audience, int expiry) {
            return new AuthToken() {
                AccessToken = this.GenerateAccessToken(username, subject, key, issuer, audience, expiry),
                RefreshToken = this.GenerateRefreshToken()
            };
        }

        public string GenerateRefreshToken() {
            return Utils.Utils.generateRandString(Utils.Constants.alphaNumeric, Constants.refreshTokenLength);
        }

    }
}
