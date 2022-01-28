using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.AircraftSchedule
{
    public class AircraftSchedulerService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftSchedulerService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<SchedulerVM> GetDetailsAsync(IHttpClientFactory httpClient, long id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"aircraftscheduler/getDetails?id={id}");

            SchedulerVM schedulerVM = new SchedulerVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM = JsonConvert.DeserializeObject<SchedulerVM>(response.Data.ToString());
            }

            return schedulerVM;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, SchedulerVM schedulerVM)
        {
            string jsonData = JsonConvert.SerializeObject(schedulerVM);

            string url = "aircraftscheduler/create";

            if (schedulerVM.Id > 0)
            {
                url = "aircraftscheduler/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<List<SchedulerVM>> ListAsync(IHttpClientFactory httpClient, SchedulerFilter schedulerFilter)
        {
            string jsonData = JsonConvert.SerializeObject(schedulerFilter);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, "aircraftscheduler/list", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<SchedulerVM>();
            }

            List<SchedulerVM> appointmentsList = JsonConvert.DeserializeObject<List<SchedulerVM>>(response.Data.ToString());

            return appointmentsList;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, long id)
        {
            string url = $"aircraftscheduler/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

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
