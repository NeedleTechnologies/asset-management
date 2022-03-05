using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asset_management.Entities
{
    public class DocumentFolder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string folderName { get; set; }
        public string folderUniqueKey { get; set; } = Guid.NewGuid().ToString("N");
        public List<Document> documents { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
    }
}