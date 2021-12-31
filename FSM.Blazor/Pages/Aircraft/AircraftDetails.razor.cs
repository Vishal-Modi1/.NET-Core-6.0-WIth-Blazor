using DataModels.VM.Aircraft;
using FSM.Blazor.Data.Aircraft;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FSM.Blazor.Pages.Aircraft
{
    partial class AircraftDetails
    {
        [Parameter]
        public string AircraftId { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Parameter]
        public AirCraftVM AircraftData { get; set; }

        public string CompanyName;

        public bool isDataLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            AircraftData = await AircraftService.GetDetailsAsync(_httpClient, Convert.ToInt32(AircraftId));
            SetCompanyName();
        }

        async Task AircraftEditDialog(int id, string title)
        {
            AirCraftVM aircraftData = await AircraftService.GetDetailsAsync(_httpClient, id);

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "aircraftData", AircraftData } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            SetCompanyName();
        }

        private void SetCompanyName()
        {
            CompanyName = AircraftData.CompanyName;

            if (string.IsNullOrEmpty(CompanyName))
            {
                CompanyName = AircraftData.Companies.Where(p => p.Id == AircraftData.CompanyId).FirstOrDefault().Name;
            }
        }
    }
}
