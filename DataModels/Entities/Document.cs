using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
        
        public string Type { get; set; }

        [Column("Size(InKB)")]
        public long Size { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public int CompanyId { get; set; }

        public long UserId { get; set; }

        public int ModuleId { get; set; }

        public bool IsActive { get; set; }
        
        public bool IsDeleted { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public long? DeletedBy { get; set; }
    }
}
