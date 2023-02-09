using System;

namespace DataModels.VM.LogBook
{
    public class LogBookFlightTimeDetailVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public double HobbsStart { get; set; }
        public double HobbsEnd { get; set; }
        public double TachStart { get; set; }
        public double TachEnd { get; set; }
        public double TimeOut { get; set; }
        public double TimeOff { get; set; }
        public double TimeOn { get; set; }
        public double TimeIn { get; set; }
        public double OnDuty { get; set; }
        public double OffDuty { get; set; }
    }
}
