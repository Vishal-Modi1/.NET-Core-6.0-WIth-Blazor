using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.AircraftEquipment
{
    public class AirCraftEquipmentsVM
    {
        public int Id { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public int AirCraftId { get; set; }
        
        [Required]
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
        public Nullable<DateTime> ManufacturerDate { get; set; } = DateTime.Now;

        [Display(Name = "Log Entry Date")]
        public Nullable<DateTime> LogEntryDate { get; set; } = DateTime.Now;

        [Display(Name = "Aircraft TT at Install")]
        public Nullable<int> AircraftTTInstall { get; set; }

        [Display(Name = "Part TT at Install")]
        public Nullable<int> PartTTInstall { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
        
        [Display(Name = "In Use?")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }

        public List<StatusVM> statusList { get; set; }
        public List<EquipmentClassificationVM> classificationList { get; set; }

    }
}
