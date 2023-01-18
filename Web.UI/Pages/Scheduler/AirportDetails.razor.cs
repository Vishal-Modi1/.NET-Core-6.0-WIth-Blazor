using DataModels.VM.ExternalAPI.Airport;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Scheduler
{
    partial class AirportDetails
    {
        public int activeAirportTabIndex { get; set; }
        public int activeAirportWeatherTabIndex { get; set; }

        [Parameter] public AirportDetailsViewModel AirportDetailInfo { get; set; }


    }
}
