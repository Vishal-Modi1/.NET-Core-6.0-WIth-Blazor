using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Reservation
{
    public class FlightTagService
    {
        private readonly HttpCaller _httpCaller;

        public FlightTagService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DropDownLargeValues>> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"flighttag/listdropdownvalues";

            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownLargeValues> flightTagsVM = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                flightTagsVM = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return flightTagsVM;
        }

        public async Task<CurrentResponse> SaveTagAsync(DependecyParams dependecyParams, FlightTagVM flightTagVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(flightTagVM);

            dependecyParams.URL = "flighttag/create";

            if (flightTagVM.Id > 0)
            {
                dependecyParams.URL = "flighttag/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }
    }
}
