using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.AirTrafficControlCenter
{
    public class AirTrafficControlCenterService
    {
        private readonly HttpCaller _httpCaller;

        public AirTrafficControlCenterService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DataModels.Entities.AirTrafficControlCenter>> ListAllAsync(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "airTrafficControlCenter/listAll";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                List<DataModels.Entities.AirTrafficControlCenter> centersList = JsonConvert.DeserializeObject<List<DataModels.Entities.AirTrafficControlCenter>>(response.Data.ToString());

                return centersList;
            }
            catch (Exception exc)
            {
                return new List<DataModels.Entities.AirTrafficControlCenter>();
            }
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, int userAirTrafficControlCenterId)
        {
            dependecyParams.URL = $"userAirTrafficControlCenter/setDefault?userAirTrafficControlCenterId={userAirTrafficControlCenterId}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<int> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "userAirTrafficControlCenter/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                int value = JsonConvert.DeserializeObject<int>(response.Data.ToString());

                return value;
            }
            catch (Exception exc)
            {
                return 0;
            }
        }
    }
}
