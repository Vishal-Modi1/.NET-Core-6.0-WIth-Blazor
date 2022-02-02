using System;

namespace DataModels.VM.Scheduler
{
    public class SchedulerEndTimeDetailsVM
    {
        public long ScheduleId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Nullable<System.DateTime> UpdatedOn { get; set; }

        public Nullable<long> UpdatedBy { get; set; }
    }
}
