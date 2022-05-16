using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CodeOnlyModels {
    
    /// <summary>
    /// A model for an invite.
    /// Contains from, to and server fields.
    /// </summary>
    public class Invite {

        public string from { get; set; }

        public string to { get; set; }

        public string server { get; set; }
    }
}
