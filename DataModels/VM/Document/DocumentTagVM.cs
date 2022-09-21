using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Document
{
    public class DocumentTagVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string TagName { get; set; }

        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}
