using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Entities
{
    public class Document : CommonField
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; } = "";
        
        public string Type { get; set; }

        [Column("Size(InKB)")]
        public long Size { get; set; }

        public long? TotalDownloads { get; set; }

        public long? TotalShares { get; set; }
        
        public DateTime? LastShareDate { get; set; }
        
        public DateTime? ExpirationDate { get; set; }

        public int CompanyId { get; set; }

        public long UserId { get; set; }

        public long? DocumentDirectoryId { get; set; }

        public long? AircraftId { get; set; }

        public bool IsPersonalDocument { get; set; }

        public bool IsShareable { get; set; }

        public int ModuleId { get; set; }
    }
}
