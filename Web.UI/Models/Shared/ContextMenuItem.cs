using DataModels.Enums;
using DataModels.VM.Scheduler;

namespace Web.UI.Models.Shared
{
    public class ContextMenuItem
    {
        public SchedulerVM ScheduleDetails { get; set; }
        public string Text { get; set; }
        public ScheduleOperations Type { get; set; }
        public string Icon { get; set; }
    }
}
