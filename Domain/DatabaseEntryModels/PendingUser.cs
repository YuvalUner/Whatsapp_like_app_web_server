using Domain;
using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    /// <summary>
    /// A model for a pending user (pending verification).
    /// contains username, password + salt and hashing algorithm for it, phone, email, nickname, user's 
    /// secret questions, verification code, time the account was created, and time the verification
    /// code was created.
    /// </summary>
    public class PendingUser {

        [Key]
        public string? username { get; set; }

        public string password { get; set; }

        public string? salt { get; set; }

        public string? hashingAlgorithm { get; set; }

        public string? phone { get; set; }

        public string? email { get; set; }

        public string? nickname { get; set; }

        public SecretQuestion? secretQuestion { get; set; }

        public string? verificationCode { get; set; }

        public DateTime timeCreated { get; set; }

        public DateTime verificationCodeCreationTime { get; set; }
    }
}
