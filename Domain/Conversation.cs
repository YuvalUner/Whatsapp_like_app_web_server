namespace Domain {

    public class Conversation {

        public int Id { get; set; }

        public string with { get; set; }

        public List<Message> messages { get; set; }
    }
}
