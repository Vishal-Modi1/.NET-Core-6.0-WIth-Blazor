using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Document
{
    public class DocumentVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Type { get; set; }

        public long Size { get; set; }

        public long? TotalDownloads { get; set; }

        public long? TotalShares { get; set; }

        public DateTime? LastShareDate { get; set; }

        public bool IsShareable { get; set; }

        public string Tags { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int CompanyId { get; set; }

        public long UserId { get; set; }

        public int ModuleId { get; set; }

        [NotMapped]
        public bool IsFromParentModule { get; set; }

        public List<DropDownValues> ModulesList { get; set; }
        public List<DropDownLargeValues> UsersList { get; set; }
        public List<DropDownValues> CompniesList { get; set; }

        public List<DocumentTagVM> DocumentTagsList { get; set; }

        public long CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public int TotalRecords { get; set; }
    }
}
