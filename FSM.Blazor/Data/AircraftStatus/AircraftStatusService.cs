using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;

namespace FSM.Blazor.Data.AircraftStatus
{
    public class AircraftStatusService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftStatusService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "aircraftstatus/listdropdownvalues";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> GetById(DependecyParams dependecyParams, byte id)
        {
            dependecyParams.URL = "aircraftstatus/getbyid?id=" + id;
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
