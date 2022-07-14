using DataModels.VM.AircraftMake;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using DE = DataModels.Entities;

namespace FSM.Blazor.Data.AircraftMake
{
    public class AircraftMakeService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftMakeService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DE.AircraftMake aircraftMake)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(aircraftMake);

            dependecyParams.URL = "aircraftmake/create";

            if (aircraftMake.Id > 0)
            {
                dependecyParams.URL = "aircraftmake/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "aircraftmake/list";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<List<AircraftMakeDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "aircraftmake/list";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<AircraftMakeDataVM>();
            }

            List<AircraftMakeDataVM> aircraftMakeList = JsonConvert.DeserializeObject<List<AircraftMakeDataVM>>(response.Data.ToString());

            return aircraftMakeList;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"aircraftmake/delete?id={id}";

            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }
    }
}
