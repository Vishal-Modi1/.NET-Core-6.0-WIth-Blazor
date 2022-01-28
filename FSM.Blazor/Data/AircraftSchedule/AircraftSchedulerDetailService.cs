using DataModels.VM;
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

            string url = "aircraftschedulerdetail/create";

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }
    }
}
