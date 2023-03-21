using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Weather
{
    public class NOAARadarMapConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public NOAARadarMapConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, NOAARadarMapConfigurationVM nOAARadarMapConfigurationVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(nOAARadarMapConfigurationVM);
            dependecyParams.URL = $"nOAARadarMapConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<NOAARadarMapConfigurationVM> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "nOAARadarMapConfiguration/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                NOAARadarMapConfigurationVM NOAARadarMapConfigurationVM = JsonConvert.DeserializeObject<NOAARadarMapConfigurationVM>(response.Data.ToString());

                return NOAARadarMapConfigurationVM;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
