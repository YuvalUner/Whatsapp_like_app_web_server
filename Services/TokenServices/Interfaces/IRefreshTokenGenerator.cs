namespace Services.TokenServices.Interfaces {

    /// <summary>
    /// A generator for refresh tokens.
    /// </summary>
    public interface IRefreshTokenGenerator {

        /// <summary>
        /// Generates a refresh token.
        /// </summary>
        /// <returns>The token generated</returns>
        public string GenerateRefreshToken();
    }
}
