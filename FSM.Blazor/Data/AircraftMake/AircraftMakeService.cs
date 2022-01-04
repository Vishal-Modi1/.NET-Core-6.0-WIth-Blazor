using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using DE = DataModels.Entities;

namespace FSM.Blazor.Data.AircraftMake
{
    public class AircraftMakeService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftMakeService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, DE.AircraftMake aircraftMake)
        {
            string jsonData = JsonConvert.SerializeObject(aircraftMake);

            string url = "aircraft/createmake";

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> ListDropdownValues(IHttpClientFactory httpClient)
        {
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, $"aircraft/makelist");

            return response;
        }
    }
}
