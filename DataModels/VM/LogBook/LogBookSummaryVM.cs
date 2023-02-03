using System;
namespace DataModels.VM.LogBook
{
    public class LogBookSummaryVM
    {
        public long Id { get; set; }

        public string Departure { get; set; }

        public string Arrival { get; set; }

        public DateTime Date { get; set; }

        public string TailNo { get; set; }
    }
}
