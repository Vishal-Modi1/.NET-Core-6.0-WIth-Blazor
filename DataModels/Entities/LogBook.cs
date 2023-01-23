using System;

namespace DataModels.Entities
{
    public class LogBook : CommonField
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long AircraftId { get; set; }

        public int CompanyId { get; set; }

        public string Departure { get; set; }

        public string Arrival { get; set; }

        public Int16 TotalTime { get; set; }

        public Int16 PIC { get; set; }

        public Int16 SIC { get; set; }

        public Int16 Night { get; set; }

        public Int16 Solo { get; set; }

        public Int16 CrossCountry { get; set; }

        public Int16 NVG { get; set; }

        public Int16 NVGOperations { get; set; }

        public string Route { get; set; }

        public int Distance { get; set; }

        public Int16 DayTakeoffs { get; set; }

        public Int16 DayLandingsFullStop { get; set; }

        public Int16 NightTakeoffs { get; set; }

        public Int16 NightLandingsFullStop { get; set; }

        public Int16 AllLandings { get; set; }

        public string Comments { get; set; }
    }
}
