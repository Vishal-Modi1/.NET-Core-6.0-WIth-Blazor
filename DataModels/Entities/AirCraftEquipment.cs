using System;

namespace DataModels.Entities
{
    public class AircraftEquipment
    {
        public long Id { get; set; }

        public int StatusId { get; set; }

        public long AirCraftId { get; set; }

        public int ClassificationId { get; set; }

        public string Item { get; set; }

        public string Model { get; set; }

        public string Make { get; set; }

        public string Manufacturer { get; set; }

        public string PartNumber { get; set; }

        public string SerialNumber { get; set; }

        public Nullable<DateTime> ManufacturerDate { get; set; } = DateTime.Now;

        public Nullable<DateTime> LogEntryDate { get; set; } = DateTime.Now;

        public Nullable<int> AircraftTTInstall { get; set; }

        public Nullable<int> PartTTInstall { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; } = true;
        
        public bool IsDeleted { get; set; }
        
        public Nullable<System.DateTime> CreatedOn { get; set; }
        
        public Nullable<int> CreatedBy { get; set; }
        
        public Nullable<int> DeletedBy { get; set; }
        
        public Nullable<System.DateTime> DeletedOn { get; set; }
        
        public Nullable<int> UpdatedBy { get; set; }
        
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}