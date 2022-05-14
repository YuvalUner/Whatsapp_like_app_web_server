using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
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
