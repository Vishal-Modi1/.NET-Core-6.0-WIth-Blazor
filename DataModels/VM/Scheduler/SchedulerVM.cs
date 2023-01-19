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
            AircraftSchedulerDetailsVM = new();
            AircraftEquipmentHobbsTimeList = new();
            AircraftEquipmentsTimeList = new();
        }

        public long Id { get; set; }
        public List<DropDownLargeValues> ScheduleActivitiesList { get; set; } = new List<DropDownLargeValues>();

        [Required(ErrorMessage = "Activity type is required")]
        public long? ScheduleActivityId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        [DateGreaterThan(nameof(StartTime), "End time must not exceed start time")]
        public DateTime EndTime { get; set; }

        public bool IsRecurring { get; set; }
        public bool IsStandBy { get; set; }

        [RequiredRangeIf(nameof(RoleId), (int)Enums.UserRole.SuperAdmin,1, int.MaxValue, "Company is required")]
        public int CompanyId { get; set; }

        public long UserId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Member is required")]
        public long Member1Id { get; set; }

        public string Member1 { get; set; }

        public int RoleId { get; set; }
        
        [UnlikeIf(nameof(IsDisplayMember2Dropdown),true, nameof(Member1Id) )]
        public long? Member2Id { get; set; }

        [NotMapped]
        public bool IsDisplayMember2Dropdown { get; set; }

        [Required(ErrorMessage = "Departure airport is required")]
        public string DepartureAirport { get; set; }
        public Guid? DepartureAirportId { get; set; }

        [Required(ErrorMessage = "Arrival airport is required")]
        //[Unlike(nameof(DepartureAirport), "Arrival airport can not be the same")]
        public string ArrivalAirport { get; set; }
        public Guid? ArrivalAirportId { get; set; }

        public long? InstructorId { get; set; }

        [Required(ErrorMessage = "Aircraft is required")]
        public long? AircraftId { get; set; }
        public string TailNo { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string DisplayTitle { get; set; }

        [Required(ErrorMessage = "Flight Category is required")]
        public int FlightCategoryId { get; set; }

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

        public string FlightTagIds { get; set; }

        [NotMapped]
        public bool IsAllDay { get; set; }
        public string CssClass { get; set; }
        public string Color { get; set; }

        [NotMapped]
        public bool IsAllowToCheckDetails { get; set; }
        public List<DropDownGuidValues> DepartureAirportsList { get; set; } = new List<DropDownGuidValues>();
        public List<DropDownGuidValues> ArrivalAirportsList { get; set; }
        public List<DropDownLargeValues> InstructorsList { get; set; }
        public List<DropDownLargeValues> AircraftsList { get; set; }
        public List<DropDownValues> CompaniesList { get; set; }
        public List<DropDownValues> FlightCategoriesList { get; set; }
        public List<DropDownLargeValues> UsersList { get; set; }
        public List<DropDownLargeValues> Member1List { get; set; }
        public List<DropDownLargeValues> Member2List { get; set; }

        public List<DropDownLargeValues> FlightTagsList { get; set; }

        public AircraftSchedulerDetailsVM AircraftSchedulerDetailsVM { get; set; }

        public List<AircraftEquipmentTimeVM> AircraftEquipmentsTimeList { get; set;}
        
        public List<AircraftScheduleHobbsTime> AircraftEquipmentHobbsTimeList { get; set; }
    }
}
