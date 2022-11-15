using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.AircraftEquipment
{
    public class AircraftEquipmentsVM : CommonField
    {
        public long Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public long AircraftId { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Classification is required")]
        [Display(Name = "Classification")]
        public int ClassificationId { get; set; }
        
        [Required]
        [Display(Name = "Item")]
        public string Item { get; set; }
        
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Make")]
        public string Make { get; set; }

        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Manufacturer Date")]
        [Required(ErrorMessage = "Manufacturer date is required")]
        public Nullable<DateTime> ManufacturerDate { get; set; } 

        [Display(Name = "Log Entry Date")]
        [Required(ErrorMessage = "Log entry date is required")]
        public Nullable<DateTime> LogEntryDate { get; set; } 

        [Display(Name = "Aircraft TT at Install")]
        public Nullable<int> AircraftTTInstall { get; set; }

        [Display(Name = "Part TT at Install")]
        public Nullable<int> PartTTInstall { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
        
        [Display(Name = "In Use?")]
        public bool IsActive { get; set; }

        public List<StatusVM> StatusList { get; set; }
        public List<EquipmentClassificationVM> ClassificationList { get; set; }

    }
}
