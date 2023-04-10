using DataModels.VM.Document;
using DataModels.VM.LogBook;
using DataModels.VM.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.VM.MobileAppVM
{
    public class DocumentDetails
    {
        public List<DocumentDataVM> DocumentDataListVM { get; set; } = new();
    }

    public class ReservationDetails
    {
        public List<ReservationDataVM> ReservationDataListVM { get; set; } = new();
    }

    public class UpcomingFlights
    {
        public List<UpcomingFlight> UpcomingFlightsList { get; set; } = new();
    }

    public class LogBookDetails
    {
        public  List<LogBookDataVM> LogBookDetailsListVM { get; set; } = new();
    }
}
