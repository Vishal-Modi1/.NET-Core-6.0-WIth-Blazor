using DataModels.VM.MobileAppVM;
using DataModels.VM.MyAccount;

namespace DataModels.VM.Dashboard
{
    public class PilotDashboardVM
    {
        public long UserId { get; set; }

        public DocumentDetails DocumentDetails { get; set; } = new ();

        public LogBookDetails LogBookDetails { get; set; } = new();

        public UpcomingFlights UpcomingFlights { get; set; } = new();
    }
}
