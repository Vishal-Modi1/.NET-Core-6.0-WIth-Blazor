using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Company
{
    public class CompanyVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
     
        [Required]
        public string Address { get; set; }

        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int TotalRecords { get; set; }
    }
}
