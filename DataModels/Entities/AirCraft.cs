using System;

namespace DataModels.Entities
{
    public class Aircraft : CommonField
    {
        public long Id { get; set; }
        public string TailNo { get; set; }
        public string ImageName { get; set; }
        public string Year { get; set; }
        public int AircraftMakeId { get; set; }
        public int AircraftModelId { get; set; }
        public byte AircraftStatusId { get; set; }
        public int AircraftCategoryId { get; set; }
        public Nullable<int> AircraftClassId { get; set; }
        public Nullable<int> FlightSimulatorClassId { get; set; }
        public int NoofEngines { get; set; }
        public int? NoofPropellers { get; set; }
        public bool IsEngineshavePropellers { get; set; }
        public bool IsEnginesareTurbines { get; set; }
        public bool TrackOilandFuel { get; set; }
        public bool IsIdentifyMeterMismatch { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
