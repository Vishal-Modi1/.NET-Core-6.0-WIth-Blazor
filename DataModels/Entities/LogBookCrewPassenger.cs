using System;

namespace DataModels.Entities
{
    public class LogBookCrewPassenger
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public long CrewOrPassengerId { get; set; }
        public string Role { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
    }
}
