using System.ComponentModel.DataAnnotations;

namespace Domain.DatabaseEntryModels {

    /// <summary>
    /// A model for a contract
    /// Contains contactOf, id, last (message), server, name (nickname) and lastDate (last seen).
    /// </summary>
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
