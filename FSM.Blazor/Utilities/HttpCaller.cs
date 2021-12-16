using Newtonsoft.Json;
using System.Text;
using DataModels.VM.Common;
using System.Net.Http.Headers;
using System.Security.Claims;
using DataModels.Constants;
using Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FSM.Blazor.Utilities
{
    public class HttpCaller
    {
        

        private static AuthenticationStateProvider _authenticationStateProvider;


        private  ConfigurationSettings _configurationSettings = ConfigurationSettings.Instance;

        public HttpCaller(AuthenticationStateProvider authenticationStateProvider = null)
        {
            _authenticationStateProvider = authenticationStateProvider;
           
        }

        public async Task<CurrentResponse> GetAsync(string url, IHttpClientFactory _httpClient)
        {

            try
            {
                string apiURL = @"https://localhost:7132/api/UserRolePermission/listbyroleid";

                var request = new HttpRequestMessage(HttpMethod.Get, apiURL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "");

                var client = _httpClient.CreateClient();

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                //string apiURL = $"{_configurationSettings.APIURL}{url}";

                //var request = new HttpRequestMessage(HttpMethod.Get, apiURL);
                //request.Headers.Clear();
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                //var client = _httpClient.CreateClient();

                //HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                //CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }
        }


        //public async Task<CurrentResponse> PostAsync(string url, string jsonData)
        //{
        //    using (_httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            string apiURL = $"{_configurationSettings.APIURL}{url}";

        //            _httpClient.BaseAddress = new Uri(apiURL);
        //            _httpClient.DefaultRequestHeaders.Accept.Clear();

        //            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

        //            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        //            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(apiURL, content);
        //            CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

        //            return response;
        //        }
        //        catch (Exception exc)
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public async Task<CurrentResponse> DeleteAsync(string url)
        //{
        //    using (_httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            string apiURL = $"{_configurationSettings.APIURL}{url}";

        //            _httpClient.BaseAddress = new Uri(apiURL);
        //            _httpClient.DefaultRequestHeaders.Accept.Clear();

        //            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

        //            HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync(apiURL);
        //            CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

        //            return response;
        //        }
        //        catch (Exception exc)
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public async Task<CurrentResponse> PostFileAsync(string url, MultipartFormDataContent fileContent)
        //{
        //    using (_httpClient = new HttpClient())
        //    {
        //        try
        //        {
        //            string apiURL = $"{_configurationSettings.APIURL}{url}";

        //            _httpClient.BaseAddress = new Uri(apiURL);
        //            _httpClient.DefaultRequestHeaders.Accept.Clear();

        //            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

        //            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync(apiURL, fileContent);
        //            CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

        //            return response;
        //        }
        //        catch (Exception exc)
        //        {
        //            return null;
        //        }
        //    }
        //}


    }
}
