using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Weather
{
    public class VFRMapConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public VFRMapConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, VFRMapConfigurationVM VFRMapConfigurationVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(VFRMapConfigurationVM);
            dependecyParams.URL = $"VFRMapConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<VFRMapConfigurationVM> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "VFRMapConfiguration/getDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                VFRMapConfigurationVM VFRMapConfigurationVM = JsonConvert.DeserializeObject<VFRMapConfigurationVM>(response.Data.ToString());

                return VFRMapConfigurationVM;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
