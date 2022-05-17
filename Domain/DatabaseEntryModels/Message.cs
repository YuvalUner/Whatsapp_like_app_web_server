using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    /// <summary>
    /// A model for a single message.
    /// Contains id, the conversation id it belongs to, created time, content, sender (bool) and type (text, img, etc...)
    /// </summary>
    public class Message {

        [Key]
        public int id { get; set; }

        public int ConversationId { get; set; }

        public DateTime created { get; set; }

        public string? content { get; set; }

        public bool sent { get; set; }

        public string? type { get; set; }
    }
}
