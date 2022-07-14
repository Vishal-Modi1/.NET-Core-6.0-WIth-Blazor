using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using DE = DataModels.Entities;
using DataModels.VM.AircraftModel;

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

            dependecyParams.URL = "aircraftmodel/create";

            if (aircraftModel.Id > 0)
            {
                dependecyParams.URL = "aircraftmodel/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "aircraftmodel/list";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<List<AircraftModelDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "aircraftmodel/list";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<AircraftModelDataVM>();
            }

            List<AircraftModelDataVM> aircraftModelList = JsonConvert.DeserializeObject<List<AircraftModelDataVM>>(response.Data.ToString());

            return aircraftModelList;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"aircraftmodel/delete?id={id}";

            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }
    }
}
