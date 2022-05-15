using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    public class Contact {


        [Key]
        public string contactOf { get; set; }

        [Key]
        public string id { get; set; }

        public string? last { get; set; }

        public string server { get; set; }

        public string name { get; set; }

        public DateTime lastdate { get; set; }
    }
}
