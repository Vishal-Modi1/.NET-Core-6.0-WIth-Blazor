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

        public DateTime Date { get; set; } 

        [Range(1, long.MaxValue, ErrorMessage = "Aircraft is required")]
        public long AircraftId { get; set; }

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Departure airport is required")]
        public string Departure { get; set; }

        [Required(ErrorMessage = "Arrival airport is required")]
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

        public List<DropDownLargeValues> CrewPassengersList { get; set; } = new();

        public LogBookTrainingDetailVM LogBookTrainingDetailVM { get; set; } = new();

        [ValidateComplexType]
        public LogBookInstrumentVM LogBookInstrumentVM { get; set; } = new();
        public LogBookFlightTimeDetailVM LogBookFlightTimeDetailVM { get; set; } = new();

        [ValidateComplexType]
        public List<LogBookCrewPassengerVM> LogBookCrewPassengersList { get; set; } = new();

        public List<LogBookFlightPhotoVM> LogBookFlightPhotosList { get; set; } = new();
    }
}
