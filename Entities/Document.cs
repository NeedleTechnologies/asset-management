using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asset_management.Entities {
    public class Document {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string uniqueDocumentName { get; set; }
        public string documentName { get; set; }
        public string documentType { get; set; }
        public string uploadedBy { get; set; }
        public int folderId { get; set; }
        [ForeignKey("folderId")]
        public DocumentFolder documentFolder { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
    }
}