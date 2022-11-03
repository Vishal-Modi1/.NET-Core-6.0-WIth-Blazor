using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Discrepancy
{
    public class DiscrepancyVM
    {
        public long Id { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Please select user")]
        public long ReportedByUserId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Please select aircraft")]
        public long AircraftId { get; set; }

        [Range(1, byte.MaxValue, ErrorMessage = "Please select status")]
        public int DiscrepancyStatusId { get; set; }
        public string FileName { get; set; }
        public string FileDisplayName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Description { get; set; }
        public string ActionTaken { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please select company")]
        public int CompanyId { get; set; }

        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }

        // Dropdown List
        public List<DropDownValues> StatusList { get; set; } = new();
        public List<DropDownLargeValues> UsersList { get; set; } = new();

        public List<DiscrepancyHistoryVM> DiscrepancyHistoryVM { get; set; } = new();
    }
}
