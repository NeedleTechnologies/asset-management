using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asset_management.Entities {
    public class Activity {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string description { get; set; }
        public string userName { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
    }
}