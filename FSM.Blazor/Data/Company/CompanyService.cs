using DataModels.VM.Common;
using DataModels.VM.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
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

        public async Task<List<CompanyVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "Company/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<CompanyVM>();
            }

            List<CompanyVM> companies = JsonConvert.DeserializeObject<List<CompanyVM>>(response.Data.ToString());

            return companies;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, CompanyVM companyVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(companyVM);
            dependecyParams.URL = "Company/create";

            if (companyVM.Id > 0)
            {
                dependecyParams.URL = "Company/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"company/delete?id={id}";

            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<List<DropDownValues>> ListDropDownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"company/listdropdownvalues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownValues> companiesList = new List<DropDownValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                companiesList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return companiesList;
        }

        public async Task<List<DropDownValues>> ListCompanyServiceDropDownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"company/listcompanyservicesdropdownvalues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownValues> companyServicesList = new List<DropDownValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                companyServicesList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return companyServicesList;
        }

        public async Task<CurrentResponse> GetDetailsAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"company/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> IsCompanyExistAsync(DependecyParams dependecyParams, int id, string name)
        {
            dependecyParams.URL = $"company/iscompanyexist?id={id}&name={name}";
            var response = await _httpCaller.PutAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UpdateCreatedByAsync(DependecyParams dependecyParams, int id, long createdBy)
        {
            dependecyParams.URL = $"company/updatecreatedby?id={id}&createdBy={createdBy}";
            var response = await _httpCaller.PutAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UploadLogo(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"company/uploadlogo";

            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }
    }
}
