using System;

namespace DataModels.Entities
{
    public class AircraftEquipment : CommonField
    {
        public long Id { get; set; }

        public int StatusId { get; set; }

        public long AircraftId { get; set; }

        public int ClassificationId { get; set; }

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
    }
}