using DataModels.VM.User;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.User
{
    public class UserService
    {
        private readonly HttpCaller _httpCaller;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<UserDataVM>> ListAsync(IHttpClientFactory httpClient, DatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync( httpClient, "user/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<UserDataVM>();
            }

            List<UserDataVM> userDataList = JsonConvert.DeserializeObject<List<UserDataVM>>(response.Data);

            return userDataList; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, UserVM userVM)
        {
            string jsonData = JsonConvert.SerializeObject(userVM);
            
            string url = "user/create";

            if (userVM.Id > 0)
            {
                url = "user/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, long id)
        {
            string url = $"user/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

        public async Task<CurrentResponse> UpdateIsUserActive(IHttpClientFactory httpClient, long id, bool isActive)
        {
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, $"user/updatestatus?id={id}&isActive={isActive}");

            return response;
        }

        public async Task<UserVM> GetDetailsAsync(IHttpClientFactory httpClient, long id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"user/getDetails?id={id}");

            UserVM userVM = new UserVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data);
            }

            return userVM;
        }

        public async Task<UserFilterVM> GetFiltersAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"user/getfilters");

            UserFilterVM userFilterVM = new UserFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userFilterVM = JsonConvert.DeserializeObject<UserFilterVM>(response.Data);
            }

            return userFilterVM;
        }

        public async Task<CurrentResponse> IsEmailExistAsync(IHttpClientFactory httpClient, string email)
        {
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, $"user/isemailexist?email={email}");

            return response;
        }
    }
}
