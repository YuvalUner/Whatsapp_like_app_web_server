using System.ComponentModel.DataAnnotations;

namespace Domain {

    public class Contact {


        [Key]
        public string contactOf { get; set; }

        [Key]
        public string name { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
