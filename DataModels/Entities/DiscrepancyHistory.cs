using System;

namespace DataModels.Entities
{
    public class DiscrepancyHistory
    {
        public long Id { get; set; }
        public long DiscrepancyId { get; set; }
        public string Message { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
