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

            dependecyParams.URL = "aircraft/createmake";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "aircraft/makelist";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
