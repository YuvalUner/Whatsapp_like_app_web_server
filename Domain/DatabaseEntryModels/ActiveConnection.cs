using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseEntryModels {

    public class ActiveConnection {

        public string Username { get; set; }

        public string ConnectionId { get; set; }

        public string UserAgent { get; set; }

        public string Identifier { get; set; }
    }
}
