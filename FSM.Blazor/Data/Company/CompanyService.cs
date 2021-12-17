using DataModels.VM.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Company
{
    public class CompanyService
    {
        private readonly HttpCaller _httpCaller;

        public CompanyService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<CompanyVM>> ListAsync(IHttpClientFactory httpClient, DatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync( httpClient, "Company/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<CompanyVM>();
            }

            List<CompanyVM> companies = JsonConvert.DeserializeObject<List<CompanyVM>>(response.Data);

            return companies; 
        }
    }
}
