using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Document
{
    public class DocumentDataVM
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ModuleName { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }
        public string Type { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? LastShareDate { get; set; }


        public bool IsShareable { get; set; }

        [NotMapped]
        public string DocumentPath { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }

        [NotMapped]
        public bool IsLoadingDownloadButton { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string Size { get; set; }

        public DateTime CreatedOn { get; set; }

        public long TotalDownloads { get; set; }
        public long TotalShares { get; set; }
        public int TotalRecords { get; set; }
    }
}
