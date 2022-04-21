using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Document;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Document
{
    public class DocumentService
    {
        private readonly HttpCaller _httpCaller;

        public DocumentService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(navigationManager, authenticationStateProvider);
        }

        public async Task<DocumentFilterVM> GetFiltersAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"document/getfilters");

            DocumentFilterVM documentFilterVM = new DocumentFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentFilterVM = JsonConvert.DeserializeObject<DocumentFilterVM>(response.Data.ToString());
            }

            return documentFilterVM;
        }

        public async Task<List<DocumentDataVM>> ListAsync(IHttpClientFactory httpClient, DatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, "document/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<DocumentDataVM>();
            }

            List<DocumentDataVM> documents = JsonConvert.DeserializeObject<List<DocumentDataVM>>(response.Data.ToString());

            return documents;
        }

        public async Task<DocumentVM> GetDetailsAsync(IHttpClientFactory httpClient, Guid id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"document/getDetails?id={id}");

            DocumentVM documentVM = new DocumentVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentVM = JsonConvert.DeserializeObject<DocumentVM>(response.Data.ToString());
            }

            return documentVM;
        }

        public async Task<List<DocumentTagVM>> GetDocumentTagsList(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"documenttag/list");

            List<DocumentTagVM> documentTagsVM = new List<DocumentTagVM>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentTagsVM = JsonConvert.DeserializeObject<List<DocumentTagVM>>(response.Data.ToString());
            }

            return documentTagsVM;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, DocumentVM documentVM)
        {
            string jsonData = JsonConvert.SerializeObject(documentVM);

            string url = "document/create";

            if (documentVM.Id !=  Guid.Empty)
            {
                url = "document/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, Guid id)
        {
            string url = $"document/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

        public async Task<CurrentResponse> UpdateTotalDownloadsAsync(IHttpClientFactory httpClient, Guid id)
        {
            string url = $"document/updatetotaldownloads";
            string jsonData = JsonConvert.SerializeObject(id);
            CurrentResponse response = await _httpCaller.PutAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> UpdateTotalSharesAsync(IHttpClientFactory httpClient, Guid id)
        {
            string url = $"document/updatetotalshares";
            string jsonData = JsonConvert.SerializeObject(id);
            CurrentResponse response = await _httpCaller.PutAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> UploadDocumentAsync(IHttpClientFactory httpClient, MultipartFormDataContent fileContent)
        {
            string url = $"document/uploadfile";

            CurrentResponse response = await _httpCaller.PostFileAsync(httpClient, url, fileContent);

            return response;
        }

        public async Task<CurrentResponse> SaveTagAsync(IHttpClientFactory httpClient, DocumentTagVM documentTagVM)
        {
            string jsonData = JsonConvert.SerializeObject(documentTagVM);

            string url = "documenttag/create";

            if (documentTagVM.Id > 0)
            {
                url = "documenttag/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }
    }
}
