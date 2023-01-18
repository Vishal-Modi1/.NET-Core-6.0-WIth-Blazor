using DataModels.Enums;
using DataModels.VM.Common;
using System;

namespace DataModels.VM.Reservation
{
    public class ReservationDataTableParams : DatatableParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? UserId { get; set; }
        public long? AircraftId { get; set; }
        public Guid DepartureAirportId { get; set; }
        public Guid ArrivalAirportId { get; set; }
        public ReservationType ReservationType { get; set; }
    }
}
