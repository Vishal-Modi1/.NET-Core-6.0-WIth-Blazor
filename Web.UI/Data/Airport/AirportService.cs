using DataModels.VM.Common;
using DataModels.VM.ExternalAPI.Airport;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Airport
{
    public class AirportService
    {
        private readonly HttpCaller _httpCaller;

        public AirportService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DropDownGuidValues>> ListDropDownValues(DependecyParams dependecyParams, AirportAPIFilter airportAPIFilter)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(airportAPIFilter);
            dependecyParams.URL = $"airport/listdropdownvalues";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);
            List<DropDownGuidValues> companiesList = new List<DropDownGuidValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                companiesList = JsonConvert.DeserializeObject<List<DropDownGuidValues>>(response.Data.ToString());
            }

            return companiesList;
        }

        public async Task<CurrentResponse> IsValid(DependecyParams dependecyParams, string airportName)
        {
            dependecyParams.URL = $"airport/isValid?airportName={airportName}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> FindByName(DependecyParams dependecyParams, string airportName)
        {
            dependecyParams.URL = $"airport/findByName?airportName={airportName}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
