using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Discrepancy
{
    public class DiscrepancyFileService
    {
        private readonly HttpCaller _httpCaller;

        public DiscrepancyFileService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<DiscrepancyFileVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"discrepancy/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            DiscrepancyFileVM discrepancyFileVM = new DiscrepancyFileVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                discrepancyFileVM = JsonConvert.DeserializeObject<DiscrepancyFileVM>(response.Data.ToString());
            }

            return discrepancyFileVM;
        }

        //public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DiscrepancyFileVM discrepancyFileVM)
        //{
        //    dependecyParams.JsonData = JsonConvert.SerializeObject(discrepancyFileVM);
        //    dependecyParams.URL = "discrepancyFile/create";

        //    if (discrepancyFileVM.Id > 0)
        //    {
        //        dependecyParams.URL = "discrepancyFile/edit";
        //    }

        //    CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

        //    return response;
        //}

        public async Task<CurrentResponse> UploadDocumentAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"discrepancyFile/uploadfile";
            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"discrepancyFile/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<List<DiscrepancyFileVM>> ListAsync(DependecyParams dependecyParams,long id)
        {
            try
            {
                dependecyParams.URL = $"discrepancyFile/list?id={id}";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                List<DiscrepancyFileVM> discrepancyFilesList = JsonConvert.DeserializeObject<List<DiscrepancyFileVM>>(response.Data.ToString());

                return discrepancyFilesList;
            }
            catch (Exception exc)
            {
                return new List<DiscrepancyFileVM>();
            }
        }
    }
}
