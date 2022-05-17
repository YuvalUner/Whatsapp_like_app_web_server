using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils {

    /// <summary>
    /// A collection of constants for frequent use.
    /// </summary>
    public class Constants {

        public static readonly int codeLength = 6;
        public static readonly int saltLength = 24;
        public static readonly int refreshTokenLength = 256;
        public static readonly string currentPasswordHash = "Pbkdf2";
        public static readonly int accessTokenTestingExpiry = 60;
        public static readonly string alphaNumericSpecial = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+~?'][<>.,";
        public static readonly string alphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly string numeric = "0123456789";
    }
}
