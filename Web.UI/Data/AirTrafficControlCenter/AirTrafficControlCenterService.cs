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
    }
}
