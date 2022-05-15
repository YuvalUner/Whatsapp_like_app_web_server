using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Domain.CodeOnlyModels;
using System.Security.Cryptography;

namespace Utils {

    public class Utils {

        public static readonly string alphaNumericSpecial = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+~?'][<>.,";
        public static readonly string alphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly string numeric = "0123456789";

        public static string generateRandString(string selectionString, int length) {
            string salt = "";
            int len = selectionString.Length;
            Random rand = new Random();
            for (int i = 0; i < length; i++) {
                salt += selectionString[rand.Next(len)];
            }
            return salt;
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

        public static string hashWithSHA256(string stringToHash) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] result = sha256.ComputeHash(new UTF8Encoding().GetBytes(stringToHash));
                return Encoding.UTF8.GetString(result, 0, result.Length);
            }
        }
    }
}
