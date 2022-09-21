using DataModels.CustomValidations;
using DataModels.Entities;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Scheduler
{
    public class SchedulerVM
    {
        public SchedulerVM()
        {
            AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM();
            AircraftEquipmentHobbsTimeList = new List<AircraftScheduleHobbsTime>();
            AircraftEquipmentsTimeList = new List<AircraftEquipmentTimeVM>();
        }

        public long Id { get; set; }
        public List<DropDownValues> ScheduleActivitiesList { get; set; }

        [Required(ErrorMessage = "Activity type is required")]
        public int? ScheduleActivityId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [DateGreaterThan(nameof(StartTime), "End time must not exceed start time")]
        public DateTime EndTime { get; set; }

        public bool IsRecurring { get; set; }
        public bool IsStandBy { get; set; }

        public List<DropDownLargeValues> Member1List { get; set; }

        public long? Member1Id { get; set; }

        public List<DropDownLargeValues> Member2List { get; set; }

        [UnlikeIf(nameof(IsDisplayMember2Dropdown),true, nameof(Member1Id) )]
        public long? Member2Id { get; set; }

        [NotMapped]
        public bool IsDisplayMember2Dropdown { get; set; }
        public List<DropDownLargeValues> InstructorsList { get; set; }

        public long? InstructorId { get; set; }

        public List<DropDownLargeValues> AircraftsList { get; set; }

        [Required]
        public long? AircraftId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string DisplayTitle { get; set; }

        public string FlightRoutes { get; set; }

        public string Comments { get; set; }

        public string InternalComments { get; set; }

        public string FlightType { get; set; }

        public string FlightRules { get; set; }

        public decimal EstHours { get; set; }

        public Guid ReservationId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public long CreatedBy { get; set; }

        public Nullable<System.DateTime> UpdatedOn { get; set; }

        public Nullable<long> UpdatedBy { get; set; }

        public Nullable<System.DateTime> DeletedOn { get; set; }

        public Nullable<long> DeletedBy { get; set; }

        public AircraftSchedulerDetailsVM AircraftSchedulerDetailsVM { get; set; }

        [NotMapped]
        public bool IsAllDay { get; set; }
        public string CssClass { get; set; } 
        public string Color { get; set; }

        public List<AircraftEquipmentTimeVM> AircraftEquipmentsTimeList { get; set;}

        public List<AircraftScheduleHobbsTime> AircraftEquipmentHobbsTimeList { get; set; }
    }
}
