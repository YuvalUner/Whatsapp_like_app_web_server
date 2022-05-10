using System.ComponentModel.DataAnnotations;

namespace Domain {

    public class Message {

        [Key]
        public int key { get; set; }

        public DateTime time { get; set; }

        public string content { get; set; }

        public bool sender { get; set; }

        public string type { get; set; }
    }
}
