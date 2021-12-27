using DataModels.VM.Aircraft;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Aircraft
{
    public class AircraftService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<AircraftDataVM>> ListAsync(IHttpClientFactory httpClient, AircraftDatatableParams datatableParams)
        {
            string url = "aircraft/list";

            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);
            List<AircraftDataVM> aircraftList = JsonConvert.DeserializeObject<List<AircraftDataVM>>(response.Data);

            return aircraftList; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, AirCraftVM aircraftVM)
        {
            string jsonData = JsonConvert.SerializeObject(aircraftVM);
            
            string url = "aircraft/create";

            if (aircraftVM.Id > 0)
            {
                url = "aircraft/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            string url = $"aircraft/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

        public async Task<AircraftFilterVM> GetFiltersAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"aircraft/getfilters");

            AircraftFilterVM aircraftFilterVM = new AircraftFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                aircraftFilterVM = JsonConvert.DeserializeObject<AircraftFilterVM>(response.Data);
            }

            return aircraftFilterVM;
        }
    }
}
