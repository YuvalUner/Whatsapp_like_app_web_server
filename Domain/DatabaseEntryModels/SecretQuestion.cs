using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    /// <summary>
    /// A model for the secret question a user is asked for resetting their password.
    /// Contains question id, question text and answer text.
    /// </summary>
    public class SecretQuestion {

        public int Id { get; set; }

        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
