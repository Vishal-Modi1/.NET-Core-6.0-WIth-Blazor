using DataModels.VM.Document;
using DataModels.VM.User;
using DataModels.VM.Reservation;
using System.Collections.Generic;

namespace DataModels.VM.MyAccount
{
    public class MyProfileDetailsVM
    {
        public UserVM UserDetails { get; set; }

        public DocumentDetails DocumentDetails { get; set; }

        public ReservationDetails ReservationDetails { get; set; }
    }

    public class DocumentDetails
    {
        public List<DocumentDataVM> DocumentDataListVM { get; set; }
    }

    public class ReservationDetails
    {
        public List<ReservationDataVM> ReservationDataListVM { get; set; }
    }
}
