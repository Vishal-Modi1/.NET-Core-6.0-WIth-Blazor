using DataModels.VM.Common;
using System.Collections.Generic;

namespace DataModels.VM.Location
{
    public class LocationVM
    {
        public int Id { get; set; }
        public string PhysicalAddress { get; set; }
        public short TimezoneId { get; set; }
        public string AirportCode { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public List<DropDownValues> Timezones { get; set; }
        public int TotalRecords { get; set; }   
    }
}
