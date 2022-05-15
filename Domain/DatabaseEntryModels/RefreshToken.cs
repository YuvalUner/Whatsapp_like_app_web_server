using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseEntryModels {

    public class RefreshToken {

        [Key]
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        // Using the name from the database. 
        public string RegisteredUserusername { get; set; }
    }
}
