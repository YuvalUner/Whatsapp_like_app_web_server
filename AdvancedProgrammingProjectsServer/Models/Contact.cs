using System.ComponentModel.DataAnnotations;

namespace AdvancedProgrammingProjectsServer.Models {

    public class Contact {


        [Key]
        public string contactOf { get; set; }

        [Key]
        public string name { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
