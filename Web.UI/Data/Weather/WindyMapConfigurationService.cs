using DataModels.Entities;
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

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, WindyMapConfiguration windyMapConfiguration)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(windyMapConfiguration);
            dependecyParams.URL = $"windyMapConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<WindyMapConfiguration> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "windyMapConfiguration/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                WindyMapConfiguration windyMapConfiguration = JsonConvert.DeserializeObject<WindyMapConfiguration>(response.Data.ToString());

                return windyMapConfiguration;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
