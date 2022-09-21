using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataModels.Entities
{
    public class SubscriptionPlan : CommonField
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ModuleIds { get; set; }

        public bool IsCombo { get; set; }

        public decimal Price { get; set; }

        [Column("Duration(InMonths)")]
        public Int16 Duration { get; set; }

        public string Description { get; set; }

    }
}
