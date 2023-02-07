using System;

namespace DataModels.VM.LogBook
{
    public class LogBookCrewPassengerVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public long? CrewOrPassengerId { get; set; }
        public long? UserId { get; set; }
        public short RoleId { get; set; }
        public bool IsSystemMember { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
