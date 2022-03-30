using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.SubscriptionPlan
{
    public class SubscriptionPlanDataVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ModuleIds { get; set; }

        public string ModulesName { get; set; }
        public bool IsCombo { get; set; }

        public decimal Price { get; set; }

        [Column("Duration(InMonths)")]
        public Int16 Duration { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }

        public bool IsActive { get; set; }

        public int TotalRecords { get; set; }
    }
}
