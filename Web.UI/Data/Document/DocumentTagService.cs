using DataModels.VM.Common;
using DataModels.VM.Document;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.Document
{
    public class DocumentTagService
    {
        private readonly HttpCaller _httpCaller;

        public DocumentTagService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<DocumentTagDataVM>> ListByCompanyId(DependecyParams dependecyParams, int companyId)
        {
            dependecyParams.URL = $"documenttag/listByCompanyId?companyId={companyId}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DocumentTagDataVM> documentTagsVM = new List<DocumentTagDataVM>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentTagsVM = JsonConvert.DeserializeObject<List<DocumentTagDataVM>>(response.Data.ToString());
            }

            return documentTagsVM;
        }

        public async Task<List<DropDownLargeValues>> ListDropdownValues(DependecyParams dependecyParams, int companyId)
        {
            dependecyParams.URL = $"documenttag/listdropdownvalues?companyId={companyId}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownLargeValues> documentTagsVM = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentTagsVM = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return documentTagsVM;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, DocumentTagVM documentTagVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(documentTagVM);

            dependecyParams.URL = "documenttag/create";

            if (documentTagVM.Id > 0)
            {
                dependecyParams.URL = "documenttag/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<DocumentTagVM> FindById(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"documenttag/findById?id={id}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            DocumentTagVM documentTagVM = new DocumentTagVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                documentTagVM = JsonConvert.DeserializeObject<DocumentTagVM>(response.Data.ToString());
            }

            return documentTagVM;
        }
    }
}
