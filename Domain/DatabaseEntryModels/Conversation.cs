namespace Domain.DatabaseEntryModels {

    /// <summary>
    /// A model for a coversation.
    /// Contains Id, with and messages list.
    /// </summary>
    public class Conversation {

        public int Id { get; set; }

        public string with { get; set; }

        public List<Message> messages { get; set; }
    }
}
