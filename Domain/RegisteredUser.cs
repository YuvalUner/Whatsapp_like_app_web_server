using System.ComponentModel.DataAnnotations;

namespace Domain {

    public class RegisteredUser {

        [Key]
        public string username { get; set; }

        public string password { get; set; }

        public string salt { get; set; }

        public string hashingAlgorithm { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string nickname { get; set; }

        public SecretQuestion secretQuestions { get; set; }

        public string? verificationcode { get; set; }

        public DateTime verificationCodeCreationTime { get; set; }

        public List<Contact> contacts { get; set; }

        public string nickNum { get; set; }

        public string? description { get; set; }

        public List<Conversation> conversations { get; set; }
    }
}
