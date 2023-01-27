using System;

namespace DataModels.VM.LogBook
{
    public class LogBookCrewPassengerVM
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public long CrewOrPassengerId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
