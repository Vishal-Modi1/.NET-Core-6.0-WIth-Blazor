using Microsoft.AspNetCore.Components;
using DataModels.VM.Reservation;
using GlobalUtilities;

namespace Web.UI.Pages.Common
{
    partial class UpcomingFlights
    {
        [Parameter] public List<UpcomingFlight> upcomingFlights { get; set; }
        [Parameter] public bool IsForAircraft { get; set; }

        protected override void OnInitialized()
        {
            isFilterBarVisible = true;
        }
    }
}
