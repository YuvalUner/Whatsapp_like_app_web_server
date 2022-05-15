using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CodeOnlyModels;

namespace Services.TokenServices.Interfaces {

    public interface IAuthTokenGenerator {

        AuthToken GenerateAuthToken(string username, string subject, string key, string issuer, string audience, int expiry);
    }
}
