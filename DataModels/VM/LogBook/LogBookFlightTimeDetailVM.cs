using System;

namespace DataModels.VM.LogBook
{
    public class LogBookFlightTimeDetailVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public Int32 HobbsStart { get; set; }
        public Int32 HobbsEnd { get; set; }
        public Int32 TachStart { get; set; }
        public Int32 TachEnd { get; set; }
        public Int32 TimeOut { get; set; }
        public Int32 TimeOff { get; set; }
        public Int32 TimeOn { get; set; }
        public Int32 TimeIn { get; set; }
        public Int32 OnDuty { get; set; }
        public Int32 OffDuty { get; set; }
    }
}
