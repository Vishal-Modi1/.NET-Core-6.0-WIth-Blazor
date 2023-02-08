using System;

namespace DataModels.Entities
{
    public class LogBookInstrumentApproach
    {
        public long Id { get; set; }
        public long LogBookInstrumentId { get; set; }
        public string Airport { get; set; }
        public short InstrumentApproachId { get; set; }
        public string Runway { get; set; }
        public bool IsCircleToLand { get; set; }
        public string Comments { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
