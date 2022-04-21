using DataModels.VM.Common;
using System;
using System.Collections.Generic;

namespace DataModels.VM.SubscriptionPlan
{
    public class SubscriptionPlanVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ModuleIds { get; set; }

        public List<DropDownValues> ModulesList { get; set; }

        public bool IsCombo { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Int16 Duration { get; set; }

        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}
