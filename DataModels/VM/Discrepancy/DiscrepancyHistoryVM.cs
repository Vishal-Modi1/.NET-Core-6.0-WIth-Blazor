using System;

namespace DataModels.VM.Discrepancy
{
    public class DiscrepancyHistoryVM
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public long DiscrepancyId { get; set; }
    }
}
