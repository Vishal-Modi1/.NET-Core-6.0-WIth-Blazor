using DataModels.VM.Common;
using DataModels.VM.Company;
using System;
using System.Collections.Generic;

namespace DataModels.VM.Reservation
{
    public class ReservationFilterVM : CommonFilterVM
    {
        public List<DropDownLargeValues> Users { get; set; } = new();
        public List<DropDownLargeValues> Aircrafts { get; set; } = new();
        public List<DropDownGuidValues> DepartureAirportsList { get; set; } = new ();
        public List<DropDownGuidValues> ArrivalAirportsList { get; set; } = new();
        public List<DropDownValues> ReservationTypeFilter { get; set; } = new();

        public long UserId { get; set; }
        public long AircraftId { get; set; }

        public Guid DepartureAirportId { get; set; }

        public Guid ArrivalAirportId { get; set; }

        public int ReservationTypeFilterId { get; set; }
    }
}
