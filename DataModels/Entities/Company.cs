using System;

namespace DataModels.Entities
{
    public class Company : CommonField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TimeZone { get; set; }
        public string Website { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }

        public string Logo { get; set; }
        public string PrimaryAirport { get; set; }
        public short? PrimaryServiceId { get; set; }
        public string ContactNo { get; set; }
        public new long? CreatedBy { get; set; }

    }
}
