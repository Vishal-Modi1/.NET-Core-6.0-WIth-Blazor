using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Discrepancy
{
    public class DiscrepancyService
    {
        private readonly HttpCaller _httpCaller;

        public DiscrepancyService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }
        
        public async Task<List<DiscrepancyDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            try
            {
                dependecyParams.URL = "discrepancy/list";
                dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
                CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

                List<DiscrepancyDataVM> discrepanciesList = JsonConvert.DeserializeObject<List<DiscrepancyDataVM>>(response.Data.ToString());

                return discrepanciesList;
            }
            catch (Exception exc)
            {
                return new List<DiscrepancyDataVM>();
            }
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DiscrepancyVM discrepancyVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(discrepancyVM);
            dependecyParams.URL = "discrepancy/create";

            if (discrepancyVM.Id > 0)
            {
                dependecyParams.URL = "discrepancy/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<List<DropDownValues>> ListStatusDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"discrepancy/listStatusDropdownValues";
            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownValues> discrepancyStatusesList = new List<DropDownValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                discrepancyStatusesList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
            }

            return discrepancyStatusesList;
        }

        public async Task<DiscrepancyVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"discrepancy/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            DiscrepancyVM discrepancyVM = new DiscrepancyVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                discrepancyVM = JsonConvert.DeserializeObject<DiscrepancyVM>(response.Data.ToString());
            }

            return discrepancyVM;
        }
    }
}
