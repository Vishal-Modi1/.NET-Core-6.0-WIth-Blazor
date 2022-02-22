using System;

namespace DataModels.VM.Reservation
{
    public class ReservationDataVM
    {
        public long Id { get; set; }

        public Guid ReservationId { get; set; }
        public string ScheduleTitle { get; set; }
        public string FlightStatus { get; set; }
        public bool? IsCheckOut { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TailNo { get; set; }
        public string CompanyName { get; set; }
        public int TotalRecords { get; set; }
    }
}
