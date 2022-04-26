using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using DE = DataModels.Entities;

namespace FSM.Blazor.Data.AircraftModel
{
    public class AircraftModelService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftModelService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DE.AircraftModel aircraftModel)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(aircraftModel);

            dependecyParams.URL = "aircraft/createModel";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "aircraft/modellist";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
