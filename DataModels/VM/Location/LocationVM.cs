using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Location
{
    public class LocationVM
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Address is required")]
        public string PhysicalAddress { get; set; }

        [Required(ErrorMessage = "Timezone is required")]
        public short TimezoneId { get; set; }

        [Required(ErrorMessage = "Airport code is required")]
        public string AirportCode { get; set; }
        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
        public List<DropDownValues> Timezones { get; set; }
        public int TotalRecords { get; set; }   
    }
}
