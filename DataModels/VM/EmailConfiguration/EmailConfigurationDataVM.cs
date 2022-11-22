using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.EmailConfiguration
{
    public class EmailConfigurationDataVM
    {
        public long Id { get; set; }
        public byte EmailTypeId { get; set; }
        public string EmailType { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
        public int TotalRecords { get; set; }
    }
}
