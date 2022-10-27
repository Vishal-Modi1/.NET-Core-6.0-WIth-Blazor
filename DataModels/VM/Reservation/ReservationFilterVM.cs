using DataModels.VM.Common;
using DataModels.VM.Company;
using System;
using System.Collections.Generic;

namespace DataModels.VM.Reservation
{
    public class ReservationFilterVM : CompanyFilterVM
    {
        public List<DropDownLargeValues> Users { get; set; } = new List<DropDownLargeValues>();
        public List<DropDownLargeValues> Aircrafts { get; set; } = new List<DropDownLargeValues>();
        public List<DropDownGuidValues> DepartureAirportsList { get; set; } = new List<DropDownGuidValues>();
        public List<DropDownGuidValues> ArrivalAirportsList { get; set; } = new List<DropDownGuidValues>();
        public long UserId { get; set; }
        public long AircraftId { get; set; }

        public Guid DepartureAirportId { get; set; }

        public Guid ArrivalAirportId { get; set; }
    }
}
