using System;

namespace DataModels.VM.Reservation
{
    public class UpcomingFlight
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string TailNo { get; set; }

        public int CompanyId { get; set; }

        public string AircraftImage { get; set; }

        public string PilotImage { get; set; }

        public string Member1 { get; set; }

        public DateTime StartDate { get; set; }
    }
}
