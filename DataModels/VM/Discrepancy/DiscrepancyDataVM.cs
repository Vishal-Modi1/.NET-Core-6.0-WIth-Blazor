using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Discrepancy
{
    public class DiscrepancyDataVM
    {
        public long Id { get; set; }
        public long ReportedByUserId { get; set; }

        public string ReportedByUser { get; set; }
        public long AircraftId { get; set; }
        public string FileName { get; set; }
        public string TailNo { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }
        public int CompanyId { get; set; }
        public byte DiscrepancyStatusId { get; set; }
        public string Status { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
        public int TotalRecords { get; set; }
    }
}
