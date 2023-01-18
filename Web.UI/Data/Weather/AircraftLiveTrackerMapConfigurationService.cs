using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Weather
{
    public class AircraftLiveTrackerMapConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftLiveTrackerMapConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfiguration)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(aircraftLiveTrackerMapConfiguration);
            dependecyParams.URL = $"aircraftLiveTrackerMapConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<AircraftLiveTrackerMapConfigurationVM> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "aircraftLiveTrackerMapConfiguration/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfiguration = JsonConvert.DeserializeObject<AircraftLiveTrackerMapConfigurationVM>(response.Data.ToString());

                return aircraftLiveTrackerMapConfiguration;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
