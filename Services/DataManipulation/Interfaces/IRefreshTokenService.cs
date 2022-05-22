using Domain.DatabaseEntryModels;

namespace Services.DataManipulation.Interfaces {

    /// <summary>
    /// A service for dealing with refresh tokens.
    /// </summary>
    public interface IRefreshTokenService {

        /// <summary>
        /// Stores a refresh token.
        /// </summary>
        /// <param name="token">The token's content</param>
        /// <param name="username">The username it belongs to.</param>
        /// <param name="userAgent">The user's user agent.</param>
        /// <returns></returns>
        public Task<bool> storeRefreshToken(string token, string username, string userAgent);

        /// <summary>
        /// Returns the refresh token matching the token given.
        /// </summary>
        /// <param name="token">The token given by the user.</param>
        /// <returns></returns>
        public Task<RefreshToken?> GetToken(string? token);

        /// <summary>
        /// Validates whether a token has already expired or not.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>True if not expired, false otherwise.</returns>
        public Task<bool> validateTokenExpiry(RefreshToken? token);

        /// <summary>
        /// Removes all of a user's previous tokens from the same userAgent, as a user should have 1 per 
        /// agent at most.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public Task<bool> RemovePreviousTokens(string? username, string? userAgent);

    }
}
