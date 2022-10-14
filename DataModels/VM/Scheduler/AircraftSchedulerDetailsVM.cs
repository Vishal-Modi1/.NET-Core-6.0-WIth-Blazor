using System;

namespace DataModels.VM.Scheduler
{
    public class AircraftSchedulerDetailsVM
    {
        public long Id { get; set; }

        public long AircraftScheduleId { get; set; }

        public string FlightStatus { get; set; }

        public DateTime? CheckInTime { get; set; }

        public Guid AirportReferenceId { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public long? CheckInBy { get; set; }

        public string CheckInByUserName { get; set; }

        public string CheckOutByUserName { get; set; }

        public long? CheckOutBy { get; set; }

        public bool IsCheckOut { get; set; }
    }
}
