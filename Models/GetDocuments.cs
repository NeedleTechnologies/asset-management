using System;
namespace asset_management.Models
{
    public class GetDocumentsResponseModel
    {
        public string uploadedBy { get; set; }
        public DateTime dateUploaded { get; set; }
        public string folderName { get; set; }
        public string documentName { get; set; }
        public string uniqueDocumentName { get; set; }
    }
}