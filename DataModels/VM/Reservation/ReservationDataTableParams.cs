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
    }
}
