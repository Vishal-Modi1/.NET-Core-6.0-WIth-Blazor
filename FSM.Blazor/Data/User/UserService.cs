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

            List<UserDataVM> companies = JsonConvert.DeserializeObject<List<UserDataVM>>(response.Data);

            return companies; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, UserDataVM userVM)
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

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            string url = $"user/delete?id={id}";
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);

            return response;
        }
    }
}
