using System;
using System.ComponentModel.DataAnnotations;
using DataModels.CustomValidations;

namespace DataModels.VM.LogBook
{
    public class LogBookCrewPassengerVM
    {
        public long Id { get; set; }

        public long LogBookId { get; set; }

        [RequiredIf(nameof(IsSystemMember), false, ErrorMessage = "Crew Passenger is required")]
        public long? CrewOrPassengerId { get; set; }

        [RequiredIf(nameof(IsSystemMember), true, ErrorMessage = "User is required")]
        public long? UserId { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Role is required")]
        public short RoleId { get; set; }
        public bool IsSystemMember { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
