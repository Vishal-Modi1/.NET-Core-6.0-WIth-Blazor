using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.LogBook
{
    public class LogBookVM : CommonField
    {
        public long Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [Range(1, long.MaxValue, ErrorMessage = "Aircraft is required")]
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

        public List<DropDownLargeValues> AircraftsList { get; set; } = new();

        public List<DropDownLargeValues> CrewPassengersList { get; set; } = new();

        public LogBookTrainingDetailVM LogBookTrainingDetailVM { get; set; } = new();

        public LogBookInstrumentVM LogBookInstrumentVM { get; set; } = new();
        public LogBookFlightTimeDetailVM LogBookFlightTimeDetailVM { get; set; } = new();

        public List<LogBookCrewPassengerVM> LogBookCrewPassengersList { get; set; } = new();

        public List<LogBookFlightPhotoVM> logBookFlightPhotosList { get; set; } = new();
    }
}
