using DataModels.VM.Common;
using DataModels.VM.Document.DocumentDirectory;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.Document
{
    public class DocumentDirectoryService
    {
        private readonly HttpCaller _httpCaller;

        public DocumentDirectoryService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DocumentDirectorySummaryVM>> ListWithCountByComapnyId(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "documentDirectory/listWithCountByComapnyId";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<DocumentDirectorySummaryVM>();
            }

            List<DocumentDirectorySummaryVM> directories = JsonConvert.DeserializeObject<List<DocumentDirectorySummaryVM>>(response.Data.ToString());

            return directories;
        }

        public async Task<List<DropDownLargeValues>> ListDropDownValuesByCompanyId(DependecyParams dependecyParams, int companyId)
        {
            dependecyParams.URL = $"documentDirectory/listDropDownValuesByCompanyId?companyId={companyId}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownLargeValues> documentTagsVM = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentTagsVM = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return documentTagsVM;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DocumentDirectoryVM documentDirectoryVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(documentDirectoryVM);

            dependecyParams.URL = "documentDirectory/create";

            if (documentDirectoryVM.Id > 0)
            {
                dependecyParams.URL = "documentDirectory/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"documentDirectory/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UploadDocumentAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"documentDirectory/uploadfile";

            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }
    }
}
