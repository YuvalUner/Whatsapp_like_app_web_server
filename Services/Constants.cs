using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {

    /// <summary>
    /// A collection of constants for frequent use in services.
    /// </summary>
    internal class Constants {

        public static readonly int codeLength = 6;
        public static readonly int saltLength = 24;
        public static readonly int refreshTokenLength = 256;
        public static readonly int accessTokenTestingExpiry = 60;
    }
}
