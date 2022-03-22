using System;

namespace DataModels.Entities
{
    public class DocumentTag
    {
        public int Id { get; set; }

        public string TagName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public long? DeletedBy { get; set; }
    }
}
