using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CodeOnlyModels {

    /// <summary>
    /// A collection of both an access and a refresh token.
    /// </summary>
    public class AuthToken {

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
