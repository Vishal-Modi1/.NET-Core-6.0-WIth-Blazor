using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Document.DocumentDirectory
{
    public class DocumentDirectoryVM 
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required") ]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int CompanyId { get; set; }

        public long CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }
    }
}
