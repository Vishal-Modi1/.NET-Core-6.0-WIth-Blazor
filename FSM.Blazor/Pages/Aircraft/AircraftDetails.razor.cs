using DataModels.VM.Aircraft;
using FSM.Blazor.Data.Aircraft;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Net;

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

        public bool isDataLoaded = false, isBusy = false;

        protected override async Task OnInitializedAsync()
        {
            AircraftData = await AircraftService.GetDetailsAsync(_httpClient, Convert.ToInt32(AircraftId));

            try
            {
                if (!string.IsNullOrEmpty(AircraftData.ImagePath))
                {
                    var webClient = new WebClient();
                    byte[] imageBytes = webClient.DownloadData(AircraftData.ImagePath);

                    AircraftData.ImagePath =   "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }

            }
            catch (Exception e)
            {

            }

            SetCompanyName();
        }

        async Task AircraftEditDialog(int id, string title)
        {
            isBusy = true;
            StateHasChanged();

            AirCraftVM aircraftData = await AircraftService.GetDetailsAsync(_httpClient, id);

            isBusy = false;
            StateHasChanged();

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
