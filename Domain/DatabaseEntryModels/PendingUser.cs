using Domain;
using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    public class PendingUser {

        [Key]
        public string username { get; set; }

        public string password { get; set; }

        public string salt { get; set; }

        public string hashingAlgorithm { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string nickname { get; set; }

        public SecretQuestion secretQuestions { get; set; }

        public string? verificationCode { get; set; }

        public DateTime timeCreated { get; set; }

        public DateTime verificationCodeCreationTime { get; set; }
    }
}
