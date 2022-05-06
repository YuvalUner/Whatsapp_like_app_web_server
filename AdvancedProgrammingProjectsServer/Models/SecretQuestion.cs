using System.ComponentModel.DataAnnotations;

namespace AdvancedProgrammingProjectsServer.Models {

    public class SecretQuestion {

        public int Id { get; set; }

        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}
