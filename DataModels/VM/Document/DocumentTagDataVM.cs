using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Document
{
    public class DocumentTagDataVM
    {
        public int Id { get; set; }

        public string TagName { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; } 

        public int TotalDocuments { get; set; }

        public int? CompanyId { get; set; }

        public long CreatedBy { get; set; }

        [NotMapped]
        public bool IsAllowToEdit { get; set; }
    }
}
