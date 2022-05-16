using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CodeOnlyModels;

namespace Services.TokenServices.Interfaces {

    /// <summary>
    /// Generates auth tokens.
    /// </summary>
    public interface IAuthTokenGenerator {

        /// <summary>
        /// Generate an auth token with the given parameters.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="subject"></param>
        /// <param name="key"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="expiry"></param>
        /// <returns>The auth token generated.</returns>
        AuthToken GenerateAuthToken(string username, string subject, string key, string issuer, string audience, int expiry = 5);
    }
}
