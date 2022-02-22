﻿using DataModels.VM.Aircraft;
using FSM.Blazor.Data.Aircraft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Radzen;
using System.Net;

namespace FSM.Blazor.Pages.Aircraft
{
    partial class AircraftDetails
    {
        public string AircraftId { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Parameter]
        public AircraftVM AircraftData { get; set; }

        public string CompanyName;

        public bool isDataLoaded = false, isBusy = false;

        protected override async Task OnInitializedAsync()
        {
            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("AircraftId", out link);

            if(link.Count() == 0 || link[0] == "")
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
            AircraftId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace("FlyManager","");
            
            AircraftData = await AircraftService.GetDetailsAsync(_httpClient, Convert.ToInt64(AircraftId));

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

            AircraftVM aircraftData = await AircraftService.GetDetailsAsync(_httpClient, id);

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
