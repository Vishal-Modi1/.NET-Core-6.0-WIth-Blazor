using Configuration;
using DataModels.Constants;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace FSM.Blazor.Utilities
{
    public class HttpCaller : ComponentBase
    {
        private static AuthenticationStateProvider _authenticationStateProvider;
        private ConfigurationSettings _configurationSettings = ConfigurationSettings.Instance;

        private static NavigationManager _navigationManager;

        public HttpCaller(NavigationManager navigationManager = null, AuthenticationStateProvider authenticationStateProvider = null)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<CurrentResponse> PostAsync(IHttpClientFactory httpClientFactory, string url, string jsonData)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var client = httpClientFactory.CreateClient("FSMAPI");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ManageUnAuthorizedError();
                    throw new Exception(HttpStatusCode.Unauthorized.ToString());
                }

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }

        }

        public async Task<CurrentResponse> PutAsync(IHttpClientFactory httpClientFactory, string url, string jsonData)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var client = httpClientFactory.CreateClient("FSMAPI");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ManageUnAuthorizedError();
                    throw new Exception(HttpStatusCode.Unauthorized.ToString());
                }

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }

        }

        public async Task<CurrentResponse> GetAsync(IHttpClientFactory _httpClient, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                var client = _httpClient.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ManageUnAuthorizedError();
                    throw new Exception(HttpStatusCode.Unauthorized.ToString());
                }

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        private void ManageUnAuthorizedError(/*IHttpClientFactory _httpClient*/)
        {
            _navigationManager.NavigateTo("/Login", true);
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory _httpClient, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                var client = _httpClient.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ManageUnAuthorizedError();
                    throw new Exception(HttpStatusCode.Unauthorized.ToString());
                }

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        public async Task<CurrentResponse> PostFileAsync(IHttpClientFactory _httpClient, string url, MultipartFormDataContent fileContent)
        {

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                request.Content = fileContent;

                var client = _httpClient.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ManageUnAuthorizedError();
                    throw new Exception(HttpStatusCode.Unauthorized.ToString());
                }

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }

        }

        public string GetClaimValue(string claimType)
        {
            if (_authenticationStateProvider == null)
            {
                return "";
            }

            ClaimsPrincipal cp = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();

            return claimValue;
        }
    }
}
