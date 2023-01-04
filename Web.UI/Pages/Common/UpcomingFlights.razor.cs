using Microsoft.AspNetCore.Components;
using DataModels.VM.Reservation;
using Utilities;

namespace Web.UI.Pages.Common
{
    partial class UpcomingFlights
    {
        [Parameter] public List<UpcomingFlight> upcomingFlights { get; set; }

        protected override void OnInitialized()
        {
            isFilterBarVisible = true;
        }

        protected override void OnParametersSet()
        {
            if (upcomingFlights == null)
            {
                return;
            }

            upcomingFlights.ForEach(p =>
            {
                p.StartDate = DateConverter.ToLocal(p.StartDate, globalMembers.Timezone);

            });
        }
    }
}
