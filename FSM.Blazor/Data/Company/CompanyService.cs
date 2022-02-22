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

            List<CompanyVM> companies = JsonConvert.DeserializeObject<List<CompanyVM>>(response.Data.ToString());

            return companies; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, CompanyVM companyVM)
        {
            string jsonData = JsonConvert.SerializeObject(companyVM);
            
            string url = "company/create";

            if (companyVM.Id > 0)
            {
                url = "company/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            string url = $"company/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }
    }
}
