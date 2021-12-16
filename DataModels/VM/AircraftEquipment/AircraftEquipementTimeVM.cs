using System;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.AircraftEquipment
{
    public class AircraftEquipmentTimeVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Equipment Name")]
        public string EquipmentName { get; set; }
        [Required]
        [Display(Name = "Hours")]
        public int Hours { get; set; }
        [Required]
        public int AircraftId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
