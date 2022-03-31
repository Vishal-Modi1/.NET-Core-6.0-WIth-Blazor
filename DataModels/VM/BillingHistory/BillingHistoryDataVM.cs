using System;

namespace DataModels.VM.BillingHistory
{
    public class BillingHistoryDataVM
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string SubscriptionPlanName { get; set; }

        public string ModuleIds { get; set; }

        public string ModulesName { get; set; }

        public bool IsCombo { get; set; }

        public decimal Price { get; set; }

        public Int16 Duration { get; set; }

        public DateTime PlanStartDate { get; set; }

        public DateTime PlanEndDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public int TotalRecords { get; set; }
    }
}
