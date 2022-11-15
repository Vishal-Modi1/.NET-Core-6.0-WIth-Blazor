using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.AircraftEquipment
{
    public class AircraftEquipmentDataVM
    {
        public long Id { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public long AircraftId { get; set; }

        public int ClassificationId { get; set; }

        public string Classification { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
        public string Item { get; set; }

        public string Model { get; set; }

        public string Make { get; set; }

        public string Manufacturer { get; set; }

        public string PartNumber { get; set; }

        public string SerialNumber { get; set; }

        public DateTime? ManufacturerDate { get; set; }

        public DateTime? LogEntryDate { get; set; } 

        public Nullable<int> AircraftTTInstall { get; set; }

        public Nullable<int> PartTTInstall { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public int TotalRecords { get; set; }
    }
}
