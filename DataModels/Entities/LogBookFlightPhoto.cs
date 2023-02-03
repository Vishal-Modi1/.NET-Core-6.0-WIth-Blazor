using System;

namespace DataModels.Entities
{
    public class LogBookFlightPhoto
    {
        public long Id { get; set; }
        public long LogBookId { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
