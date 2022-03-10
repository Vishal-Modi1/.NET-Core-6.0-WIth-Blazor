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

        public DateTime? ExpirationDate { get; set; }

        public int CompanyId { get; set; }

        public long UserId { get; set; }

        public int ModuleId { get; set; }

        [NotMapped]
        public bool IsFromParentModule { get; set; }

        public List<DropDownValues> ModulesList { get; set; }

        public long CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public int TotalRecords { get; set; }
    }
}
