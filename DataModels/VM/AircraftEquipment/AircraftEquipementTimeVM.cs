using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.AircraftEquipment
{
    public class AircraftEquipmentTimeVM
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Equipment Name")]
        public string EquipmentName { get; set; }
        [Required]
        [Display(Name = "Hours")]
        public decimal Hours { get; set; }
        [Required]
        public long AircraftId { get; set; }
        public long AircraftScheduleId { get; set; }
        public decimal InTime { get; set; }
        public decimal TotalHours { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
    }

    public class AircraftEquipmentTimeRequestVM
    {
        public List<AircraftEquipmentTimeVM> Data { get; set; }
    }
}
