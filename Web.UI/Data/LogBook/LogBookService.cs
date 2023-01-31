using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.LogBook
{
    public class LogBookService
    {
        private readonly HttpCaller _httpCaller;

        public LogBookService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DropDownSmallValues>> ListInstrumentApproachesDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"logBook/listInstrumentApproachesDropdownValues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownSmallValues> instrumentApproaches = new List<DropDownSmallValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                instrumentApproaches = JsonConvert.DeserializeObject<List<DropDownSmallValues>>(response.Data.ToString());
            }

            return instrumentApproaches;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, LogBookVM logBookVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(logBookVM);
            dependecyParams.URL = "logBook/create";

            if (logBookVM.Id > 0)
            {
                dependecyParams.URL = "logBook/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }
    }
}
