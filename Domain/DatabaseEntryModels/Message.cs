using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    public class Message {

        [Key]
        public int id { get; set; }

        public int ConversationId { get; set; }

        public DateTime created { get; set; }

        public string content { get; set; }

        public bool sent { get; set; }

        public string type { get; set; }
    }
}
