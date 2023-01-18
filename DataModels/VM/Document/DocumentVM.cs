using DataModels.CustomValidations;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Document
{
    public class DocumentVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Document name is required")]
        public string DisplayName { get; set; }

        public string Type { get; set; }

        public long Size { get; set; }

        public long? TotalDownloads { get; set; }

        public long? TotalShares { get; set; }

        [RequiredIf("IsShareable", true, ErrorMessage = "Last share date is required")]
        public DateTime? LastShareDate { get; set; }

        public bool IsShareable { get; set; }

        public long? AircraftId { get; set; }

        public bool IsPersonalDocument { get; set; }

        public string Tags { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Company is required")]
        public int CompanyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "User is required")]
        public long UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Document type is required")]
        public int ModuleId { get; set; }

        [NotMapped]
        public bool IsFromParentModule { get; set; }

        public List<DropDownValues> ModulesList { get; set; }
        public List<DropDownLargeValues> UsersList { get; set; }
        public List<DropDownValues> CompniesList { get; set; }

        public List<DropDownLargeValues> DocumentTagsList { get; set; }

        public long CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public int TotalRecords { get; set; }
    }
}
