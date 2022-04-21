using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DE = DataModels.Entities;

namespace FSM.Blazor.Data.AircraftModel
{
    public class AircraftModelService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftModelService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(navigationManager, authenticationStateProvider);
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, DE.AircraftModel aircraftModel)
        {
            string jsonData = JsonConvert.SerializeObject(aircraftModel);

            string url = "aircraft/createModel";

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> ListDropdownValues(IHttpClientFactory httpClient)
        {
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, $"aircraft/modellist");

            return response;
        }
    }
}
