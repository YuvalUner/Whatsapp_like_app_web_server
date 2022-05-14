using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Services {
    internal class Utils {

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
            client.Send(message);

        }
    }
}
