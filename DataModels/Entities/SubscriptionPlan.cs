using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataModels.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ModuleIds { get; set; }

        public bool IsCombo { get; set; }

        public decimal Price { get; set; }

        [Column("Duration(InMonths)")]
        public Int16 Duration { get; set; }

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
