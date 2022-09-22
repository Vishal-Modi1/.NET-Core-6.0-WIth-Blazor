using DataModels.VM.Common;
using System;
using System.Collections.Generic;

namespace DataModels.VM.Scheduler
{
    public class SchedulerFilter 
    {
        public int CompanyId { get; set; }

        public List<DropDownValues> Companies { get; set; }

        public DateTime StartTime { get; set; }
     
        public DateTime EndTime { get; set; }
    }
}
