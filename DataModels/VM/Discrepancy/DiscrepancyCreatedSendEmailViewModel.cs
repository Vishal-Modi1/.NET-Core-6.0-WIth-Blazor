using System;
using System.Collections.Generic;

namespace DataModels.VM.Discrepancy
{
    public class DiscrepancyCreatedSendEmailViewModel
    {
        public string UserName { get; set; }
        public string Aircraft { get; set; }
        public string ReportedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }
        public string Subject { get; set; }
        public List<string> ToEmails { get; set; } = new();
    }
}
