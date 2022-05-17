using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Location
{
    public class LocationDataVM
    {
        public int Id { get; set; }
        public string PhysicalAddress { get; set; }
        public short TimezoneId { get; set; }
        public string Timezone { get; set; }
        public string Offset { get; set; }
        public string AirportCode { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }   
        public int TotalRecords { get; set; }   
    }
}
