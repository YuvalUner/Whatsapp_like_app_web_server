using System.ComponentModel.DataAnnotations;

namespace Domain {

    public class Message {

        [Key]
        public int id { get; set; }

        public DateTime created { get; set; }

        public string content { get; set; }

        public bool sent { get; set; }

        public string type { get; set; }
    }
}
