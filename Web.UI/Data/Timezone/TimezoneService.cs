using DataModels.VM.Common;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.Timezone
{
    public class TimezoneService
    {
        private readonly HttpCaller _httpCaller;

        public TimezoneService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DropDownValues>> ListDropDownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"timezone/listdropdownvalues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownValues> companiesList = new List<DropDownValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                companiesList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return companiesList;
        }
    }
}
