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

        public double TotalTime { get; set; }

        public double PIC { get; set; }

        public double SIC { get; set; }

        public double Night { get; set; }

        public double Solo { get; set; }

        public double CrossCountry { get; set; }

        public double NVG { get; set; }

        public double NVGOperations { get; set; }

        public string Route { get; set; }

        public double Distance { get; set; }

        public short DayTakeoffs { get; set; }

        public short DayLandingsFullStop { get; set; }

        public short NightTakeoffs { get; set; }

        public short NightLandingsFullStop { get; set; }

        public short AllLandings { get; set; }

        public string Comments { get; set; }
    }
}
