using System;

namespace DataModels.Entities
{
    public class AircraftEquipmentTime
    {
        public long Id { get; set; }
        public string EquipmentName { get; set; }
        public decimal Hours { get; set; }
        public long AircraftId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
