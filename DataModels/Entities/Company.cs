using System;

namespace DataModels.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TimeZone { get; set; }
        public string Website { get; set; }
        public string PrimaryAirport { get; set; }
        public short? PrimaryServiceId { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public Nullable<DateTime> DeletedOn { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
