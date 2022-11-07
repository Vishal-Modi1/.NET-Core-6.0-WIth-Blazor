using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Discrepancy
{
    public class DiscrepancyFileVM
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public long DiscrepancyId { get; set; }
        public string AddedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public bool IsLoadingEditButton { get; set; }
    }
}
