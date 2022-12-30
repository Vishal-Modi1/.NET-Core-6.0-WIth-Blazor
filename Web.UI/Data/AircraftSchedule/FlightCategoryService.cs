using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.AircraftSchedule
{
    public class FlightCategoryService
    {
        private readonly HttpCaller _httpCaller;

        public FlightCategoryService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, FlightCategory flightCategory)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(flightCategory);
            dependecyParams.URL = "flightCategory/create";

            if (flightCategory.Id > 0)
            {
                dependecyParams.URL = "flightCategory/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"flightCategory/delete?id={id}";

            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<List<FlightCategory>> ListAll(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"flightCategory/listall";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<FlightCategory> categoriesList = new List<FlightCategory>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                categoriesList = JsonConvert.DeserializeObject<List<FlightCategory>>(response.Data.ToString());
            }

            return categoriesList;
        }
        public async Task<List<DropDownValues>> ListDropDownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"flightCategory/listdropdownvalues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownValues> categoriesList = new List<DropDownValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                categoriesList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return categoriesList;
        }
    }
}
