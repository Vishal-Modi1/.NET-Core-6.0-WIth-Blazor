using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Weather
{
    public class RadarMapConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public RadarMapConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, RadarMapConfigurationVM radarMapConfigurationVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(radarMapConfigurationVM);
            dependecyParams.URL = $"radarMapConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<RadarMapConfigurationVM> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "radarMapConfiguration/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                RadarMapConfigurationVM RadarMapConfigurationVM = JsonConvert.DeserializeObject<RadarMapConfigurationVM>(response.Data.ToString());

                return RadarMapConfigurationVM;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
