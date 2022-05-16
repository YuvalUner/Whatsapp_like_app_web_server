using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CodeOnlyModels {
    /// <summary>
    /// A model for transferring a message from one server to another.
    /// Contains from, to and content.
    /// </summary>
    public class Transfer {

        public string from { get; set; }

        public string to { get; set; }

        public string content { get; set; }
    }
}
