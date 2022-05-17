using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DatabaseEntryModels {

    /// <summary>
    /// A model for a refresh token.
    /// Contains id, token string, expiry date, username it belongs to, and user agent (browser).
    /// </summary>
    public class RefreshToken {

        [Key]
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }

        // Using the name from the database. 
        public string? RegisteredUserusername { get; set; }

        public string? UserAgent { get; set; }
    }
}
