using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Weather
{
    public class WindyMapConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public WindyMapConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, WindyMapConfigurationVM windyMapConfigurationVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(windyMapConfigurationVM);
            dependecyParams.URL = $"windyMapConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<WindyMapConfigurationVM> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "windyMapConfiguration/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                WindyMapConfigurationVM windyMapConfigurationVM = JsonConvert.DeserializeObject<WindyMapConfigurationVM>(response.Data.ToString());

                return windyMapConfigurationVM;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
