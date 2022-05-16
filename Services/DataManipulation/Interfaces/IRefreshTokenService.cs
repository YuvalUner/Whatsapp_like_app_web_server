using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DatabaseEntryModels;

namespace Services.DataManipulation.Interfaces {


    public interface IRefreshTokenService {

        public Task<bool> storeRefreshToken(string token, string username, string userAgent);

        public Task<RefreshToken?> GetToken(string? token);

        public Task<bool> validateTokenExpiry(RefreshToken? token);

        public Task<bool> RemovePreviousTokens(string? username, string? userAgent);

        //public Task<RefreshToken> renewRefreshToken(RefreshToken token);

    }
}
