using DataModels.VM.Document;
using DataModels.VM.User;
using DataModels.VM.Reservation;
using System.Collections.Generic;
using DataModels.VM.MobileAppVM;

namespace DataModels.VM.MyAccount
{
    public class MyProfileDetailsVM
    {
        public UserVM UserDetails { get; set; }

        public DocumentDetails DocumentDetails { get; set; }

        public ReservationDetails ReservationDetails { get; set; }
    }
}
