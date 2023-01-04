using System;

namespace DataModels.VM.Reservation
{
    public class UpcomingFlight
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }
    }
}
