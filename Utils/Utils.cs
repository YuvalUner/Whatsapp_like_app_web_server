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

        public static JwtSecurityToken generateJwtToken(string username, string subject, string key, string issuer, string audience, int expiry) {

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("username", username)
                };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var mac = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expiry),
                signingCredentials: mac
                );
            return token;
        }
    }
}
