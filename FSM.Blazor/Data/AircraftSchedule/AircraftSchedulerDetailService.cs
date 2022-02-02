using DataModels.VM;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.AircraftSchedule
{
    public class AircraftSchedulerDetailService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftSchedulerDetailService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> CheckOut(IHttpClientFactory httpClient, long scheduleId)
        {
            string jsonData = JsonConvert.SerializeObject(scheduleId);

            string url = "aircraftschedulerdetail/checkout";

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> UnCheckOut(IHttpClientFactory httpClient, long scheduleId)
        {
            string jsonData = JsonConvert.SerializeObject(scheduleId);

            string url = "aircraftschedulerdetail/uncheckout";

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> CheckIn(IHttpClientFactory httpClient, List<AircraftEquipmentTimeVM> aircraftEquipmentsTimeList)
        {
            string jsonData = JsonConvert.SerializeObject(aircraftEquipmentsTimeList);

            string url = "aircraftschedulerdetail/checkin";

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }


        public async Task<CurrentResponse> IsAircraftAlreadyCheckOutAsync(IHttpClientFactory httpClient, long aircraftId)
        {
            string url = $"aircraftschedulerdetail/isaircraftalreadycheckout?aircraftId={aircraftId}";
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);

            return response;
        }
    }
}
