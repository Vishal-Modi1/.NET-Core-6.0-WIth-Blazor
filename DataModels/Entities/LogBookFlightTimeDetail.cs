using System;

namespace DataModels.Entities
{
    public class LogBookFlightTimeDetail
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public short HobbsStart { get; set; }
        public short HobbsEnd { get; set; }
        public short TachStart { get; set; }
        public short TachEnd { get; set; }
        public short TimeOut { get; set; }
        public short TimeOff { get; set; }
        public short TimeOn { get; set; }
        public short TimeIn { get; set; }
        public short OnDuty { get; set; }
        public short OffDuty { get; set; }
    }
}
