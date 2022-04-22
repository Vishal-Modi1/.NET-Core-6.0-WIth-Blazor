using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.ModuleDetail
{
    public class ModuleDetailsService
    {
        private readonly HttpCaller _httpCaller;

        public ModuleDetailsService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DropDownValues>> ListDropDownValues(IHttpClientFactory httpClient)
        {
            string url = $"moduledetails/listdropdownvalues";

            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);
            List<DropDownValues> companiesList = new List<DropDownValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                companiesList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return companiesList;
        }
    }
}
