using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CodeOnlyModels {

    /// <summary>
    /// A collection of fields needed for sending an email.
    /// Contains who to email to, email subject, email body, as well as email, password of sender 
    /// and host + port of smtp service.
    /// </summary>
    public class MailRequest {

        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }
}
