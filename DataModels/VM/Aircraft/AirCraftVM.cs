using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Aircraft
{
    public class AircraftVM
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Tail no is required")]
        [Display(Name = "Tail No")]
        public string TailNo { get; set; }

        public string ImageName { get; set; }

        public string ImagePath { get; set; }

        [Display(Name = "Aircraft Image")]
        public string File { get; set; }
        [Display(Name = "year")]
        public string Year { get; set; }

        [Range(1, byte.MaxValue,ErrorMessage = "Aircraft status is required")]
        public byte AircraftStatusId { get; set; }

        [Range(1, byte.MaxValue, ErrorMessage = "Aircraft make is required")]
        [Display(Name = "Make")]
        public int AircraftMakeId { get; set; }

        [Range(1, byte.MaxValue, ErrorMessage = "Aircraft model is required")]
        [Display(Name = "Model")]
        public int AircraftModelId { get; set; }

        [Range(1, byte.MaxValue, ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int AircraftCategoryId { get; set; }

        [Required(ErrorMessage = "Class is required")]
        [Display(Name = "Class")]
        public Nullable<int> AircraftClassId { get; set; }

        [Required(ErrorMessage = "Flight Simulator is required")]
        [Display(Name = "Flight Simulator")]
        public Nullable<int> FlightSimulatorClassId { get; set; }

        [Display(Name = "Engines")]
        public int NoofEngines { get; set; }
        
        [Display(Name = "No of Propellers")]
        public int? NoofPropellers { get; set; }

        [Display(Name = "Engines have Propellers")]
        public bool IsEngineshavePropellers { get; set; }

        [Display(Name = "Engines are Turbines")]
        public bool IsEnginesareTurbines { get; set; }

        [Display(Name = "Track Oil and Fuel")]
        public bool TrackOilandFuel { get; set; }

        [Display(Name = "Identify Meter Mismatch")]
        public bool IsIdentifyMeterMismatch { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public int? CompanyId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }

        //Dropdowns list

        public List<DropDownValues> AircraftMakeList { get; set; }
        public List<DropDownValues> AircraftModelList { get; set; }
        public List<DropDownValues> AircraftCategoryList { get; set; }
        public List<DropDownValues> AircraftClassList { get; set; }
        public List<DropDownValues> FlightSimulatorClassList { get; set; }
        public List<DropDownValues> Companies { get; set; }
        public List<DropDownValues> AircraftStatusList { get; set; }
        public List<AircraftEquipmentTime> AircraftEquipmentTimesList { get; set; }

        public List<Entities.AircraftEquipment>  AirCraftEquipmentList { get; set; }

        public int TotalRecords { get; set; }

        public bool IsEquipmentTimesListChanged { get; set; }

        public AircraftStatus AircraftStatus { get; set; }

    }
}
