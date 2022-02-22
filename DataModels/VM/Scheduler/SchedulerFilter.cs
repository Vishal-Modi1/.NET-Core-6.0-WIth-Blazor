using System;

namespace DataModels.VM.Scheduler
{
    public class SchedulerFilter
    {
        public int CompanyId { get; set; }  

        public DateTime StartTime { get; set; }
     
        public DateTime EndTime { get; set; }
    }
}
