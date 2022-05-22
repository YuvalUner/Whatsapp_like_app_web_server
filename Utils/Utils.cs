using System.Text;
using System.Net;
using System.Net.Mail;
using Domain.CodeOnlyModels;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Utils {

    /// <summary>
    /// Where all methods with no actual specific affiliation go.
    /// </summary>
    public class Utils {

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="selectionString">The string to choose from.</param>
        /// <param name="length">The length of the generated string.</param>
        /// <returns></returns>
        public static string generateRandString(string selectionString, int length) {
            string s = "";
            int len = selectionString.Length;
            Random rand = new Random();
            for (int i = 0; i < length; i++) {
                s += selectionString[rand.Next(len)];
            }
            return s;
        }

        public static void sendEmail(MailRequest mail) {

            MailMessage message = new MailMessage(mail.Email, mail.ToEmail);
            message.Subject = mail.Subject;
            message.Body = mail.Body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(mail.Host, mail.Port); //Gmail smtp    
            NetworkCredential basicCredential1 = new NetworkCredential(mail.Email, mail.Password);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.SendMailAsync(message);

        }

        /// <summary>
        /// A simple function for hashing a string using SHA256.
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <returns></returns>
        public static string hashWithSHA256(string stringToHash) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] result = sha256.ComputeHash(new UTF8Encoding().GetBytes(stringToHash));
                return Encoding.UTF8.GetString(result, 0, result.Length);
            }
        }

        /// <summary>
        /// A simple method for hashing using Pbkdf2.
        /// Currently used for password hashing, should be switched to Argon2 when we can install
        /// NuGet packages for it.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashWithPbkdf2(string password, string salt) {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: Encoding.UTF8.GetBytes(salt),
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 100000,
                            numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
