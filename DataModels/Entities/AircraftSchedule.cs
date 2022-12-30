using System;

namespace DataModels.Entities
{
    public class AircraftSchedule : CommonField
    {
        public long Id { get; set; }

        public int SchedulActivityTypeId { get; set; }
        public int FlightCategoryId { get; set; }

        public Guid ReservationId { get; set; }
    
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Guid? DepartureAirportId { get; set; }

        public string DepartureAirportName { get; set; }

        public Guid? ArrivalAirportId { get; set; }

        public string ArrivalAirportName { get; set; }

        public bool IsRecurring { get; set; }

        public long Member1Id { get; set; }

        public long? Member2Id { get; set; }

        public long? InstructorId { get; set; }

        public long? AircraftId { get; set; }

        public string ScheduleTitle { get; set; }

        public string FlightType { get; set; }

        public string FlightRules { get; set; }

        public decimal EstFlightHours { get; set; }

        public string FlightRoutes { get; set; }
        
        public string Comments { get; set; }
        
        public string PrivateComments { get; set; }
        
        public bool StandBy { get; set; }

    }
}
