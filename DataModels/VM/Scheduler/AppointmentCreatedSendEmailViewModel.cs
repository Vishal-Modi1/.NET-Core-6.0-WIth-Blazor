using System;

namespace DataModels.VM.Scheduler
{
    public class AppointmentCreatedSendEmailViewModel
    {
        public string Message { get; set; }
        public string Member1 { get; set; }
        public string Member2 { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public string Aircraft { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string ActivityType { get; set; }
        public DateTime StartTime { get; set; }  
        public DateTime EndTime { get; set; }

    }
}
