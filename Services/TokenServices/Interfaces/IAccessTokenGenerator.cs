using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TokenServices.Interfaces {

    public interface IAccessTokenGenerator {

        string GenerateAccessToken(string username, string subject, string key, string issuer, string audience, int expiry);
    }
}
