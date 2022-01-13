using System;

namespace DataModels.Entities
{
    public class Aircraft
    {
        public long Id { get; set; }
        public string TailNo { get; set; }
        public string ImageName { get; set; }
        public string Year { get; set; }
        public int AircraftMakeId { get; set; }
        public int AircraftModelId { get; set; }
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
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
