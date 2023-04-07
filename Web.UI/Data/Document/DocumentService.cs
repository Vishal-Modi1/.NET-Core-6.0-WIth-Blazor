using DataModels.VM.Common;
using DataModels.VM.Document;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.Document
{
    public class DocumentService
    {
        private readonly HttpCaller _httpCaller;

        public DocumentService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<DocumentFilterVM> GetFiltersAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"document/getfilters";

            var response = await _httpCaller.GetAsync(dependecyParams);

            DocumentFilterVM documentFilterVM = new DocumentFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentFilterVM = JsonConvert.DeserializeObject<DocumentFilterVM>(response.Data.ToString());
            }

            return documentFilterVM;
        }

        public async Task<List<DocumentDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "document/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<DocumentDataVM>();
            }

            List<DocumentDataVM> documents = JsonConvert.DeserializeObject<List<DocumentDataVM>>(response.Data.ToString());

            return documents;
        }

        public async Task<DocumentVM> GetDetailsAsync(DependecyParams dependecyParams, Guid id)
        {
            dependecyParams.URL = $"document/getDetails?id={id}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            DocumentVM documentVM = new DocumentVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentVM = JsonConvert.DeserializeObject<DocumentVM>(response.Data.ToString());
            }

            return documentVM;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DocumentVM documentVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(documentVM);

            dependecyParams.URL = "document/create";

            if (documentVM.Id !=  Guid.Empty)
            {
                dependecyParams.URL = "document/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, Guid id)
        {
            dependecyParams.URL = $"document/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UpdateTotalDownloadsAsync(DependecyParams dependecyParams, Guid id)
        {
            dependecyParams.URL = $"document/updatetotaldownloads";
            dependecyParams.JsonData = JsonConvert.SerializeObject(id);
            CurrentResponse response = await _httpCaller.PutAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UpdateTotalSharesAsync(DependecyParams dependecyParams, Guid id)
        {
            dependecyParams.URL = $"document/updatetotalshares";
            dependecyParams.JsonData = JsonConvert.SerializeObject(id);
            CurrentResponse response = await _httpCaller.PutAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UploadDocumentAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"document/uploadfile";

            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }
    }
}
