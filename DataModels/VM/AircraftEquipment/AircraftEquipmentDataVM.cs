using System;

namespace DataModels.VM.AircraftEquipment
{
    public class AircraftEquipmentDataVM
    {
        public long Id { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public int AirCraftId { get; set; }

        public int ClassificationId { get; set; }

        public string Classification { get; set; }

        public string Item { get; set; }

        public string Model { get; set; }

        public string Make { get; set; }

        public string Manufacturer { get; set; }

        public string PartNumber { get; set; }

        public string SerialNumber { get; set; }

        public Nullable<DateTime> ManufacturerDate { get; set; }

        public Nullable<DateTime> LogEntryDate { get; set; } 

        public Nullable<int> AircraftTTInstall { get; set; }

        public Nullable<int> PartTTInstall { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public int TotalRecords { get; set; }
    }
}
