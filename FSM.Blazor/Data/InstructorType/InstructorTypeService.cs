using DataModels.VM.InstructorType;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Data.InstructorType
{
    public class InstructorTypeService
    {
        private readonly HttpCaller _httpCaller;

        public InstructorTypeService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(navigationManager, authenticationStateProvider);
        }

        public async Task<List<InstructorTypeVM>> ListAsync(IHttpClientFactory httpClient, DatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync( httpClient, "instructorType/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<InstructorTypeVM>();
            }

            List<InstructorTypeVM> instructorTypes = JsonConvert.DeserializeObject<List<InstructorTypeVM>>(response.Data.ToString());

            return instructorTypes; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, InstructorTypeVM instructorTypeVM)
        {
            string jsonData = JsonConvert.SerializeObject(instructorTypeVM);
            
            string url = "instructorType/create";

            if (instructorTypeVM.Id > 0)
            {
                url = "instructorType/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            
            string url = $"instructorType/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }
    }
}
