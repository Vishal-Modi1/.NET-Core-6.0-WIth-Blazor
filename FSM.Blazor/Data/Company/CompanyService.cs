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

        public async Task<List<DropDownValues>> ListDropDownValues(IHttpClientFactory httpClient)
        {
            string url = $"company/listdropdownvalues";

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
