using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TokenServices.Interfaces {

    public interface IRefreshTokenGenerator {

        public string GenerateRefreshToken();
    }
}
