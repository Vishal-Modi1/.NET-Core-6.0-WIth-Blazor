using System;

namespace DataModels.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string PhysicalAddress { get; set; }
        public short TimezoneId { get; set; }
        public string AirportCode { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
