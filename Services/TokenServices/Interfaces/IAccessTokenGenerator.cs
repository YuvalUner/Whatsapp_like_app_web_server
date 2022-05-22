namespace Services.TokenServices.Interfaces {

    /// <summary>
    /// A generator for access tokens.
    /// </summary>
    public interface IAccessTokenGenerator {

        /// <summary>
        /// Generates an access token with the given parameters
        /// </summary>
        /// <param name="username"></param>
        /// <param name="subject"></param>
        /// <param name="key"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="expiry"></param>
        /// <returns>The generated access token</returns>
        string GenerateAccessToken(string username, string subject, string key, string issuer, string audience, int expiry = 5);
    }
}
