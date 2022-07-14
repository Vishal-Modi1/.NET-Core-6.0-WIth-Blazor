using DataModels.VM.Common;
using DataModels.VM.Location;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Location
{
    public class LocationService
    {
        private readonly HttpCaller _httpCaller;

        public LocationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<LocationDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "Location/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<LocationDataVM>();
            }

            List<LocationDataVM> locationsList = JsonConvert.DeserializeObject<List<LocationDataVM>>(response.Data.ToString());

            return locationsList;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, LocationVM locationVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(locationVM);
            dependecyParams.URL = "Location/create";

            if (locationVM.Id > 0)
            {
                dependecyParams.URL = "Location/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"location/delete?id={id}";

            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<List<DropDownValues>> ListDropDownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"location/listdropdownvalues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownValues> locationsList = new List<DropDownValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                locationsList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return locationsList;
        }

        public async Task<CurrentResponse> GetDetailsAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"location/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
