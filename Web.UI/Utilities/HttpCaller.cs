using DataModels.Constants;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using DataModels.Models;

namespace Web.UI.Utilities
{
    public class HttpCaller : ComponentBase
    {
        private static AuthenticationStateProvider _authenticationStateProvider;
        private CurrentResponse _response;

        public HttpCaller(AuthenticationStateProvider authenticationStateProvider = null)
        {
            if (_authenticationStateProvider == null)
            {
                _authenticationStateProvider = authenticationStateProvider;
            }
        }

        public async Task<CurrentResponse> PostAsync(DependecyParams dependecyParams)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, dependecyParams.URL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken, dependecyParams.AuthenticationStateProvider));

                request.Content = new StringContent(dependecyParams.JsonData, Encoding.UTF8, "application/json");

                var client = dependecyParams.HttpClientFactory.CreateClient("FSMAPI");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                ManageResponse(httpResponseMessage);

                return _response;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public async Task<CurrentResponse> PutAsync(DependecyParams dependecyParams)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, dependecyParams.URL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken, dependecyParams.AuthenticationStateProvider));

                request.Content = new StringContent(dependecyParams.JsonData, Encoding.UTF8, "application/json");

                var client = dependecyParams.HttpClientFactory.CreateClient("FSMAPI");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                ManageResponse(httpResponseMessage);

                return _response;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        public async Task<CurrentResponse> GetAsync(DependecyParams dependecyParams)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, dependecyParams.URL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken, dependecyParams.AuthenticationStateProvider));

                var client = dependecyParams.HttpClientFactory.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                ManageResponse(httpResponseMessage);

                return _response;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        //private async Task ManageUnAuthorizedErrorAsync(IHttpClientFactory httpClientFactory)
        //{
        //    throw new Exception(HttpStatusCode.Unauthorized.ToString());
        //}

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, dependecyParams.URL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken, dependecyParams.AuthenticationStateProvider));

                var client = dependecyParams.HttpClientFactory.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                ManageResponse(httpResponseMessage);

                return _response;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public async Task<CurrentResponse> PostFileAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, dependecyParams.URL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken, dependecyParams.AuthenticationStateProvider));
                request.Content = fileContent;

                var client = dependecyParams.HttpClientFactory.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                ManageResponse(httpResponseMessage);

                return _response;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public string GetClaimValue(string claimType, AuthenticationStateProvider authenticationStateProvider)
        {
            if(authenticationStateProvider == null)
            {
                return "";
            }

            ClaimsPrincipal cp = authenticationStateProvider.GetAuthenticationStateAsync().Result.User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();

            return claimValue;
        }

        private void ManageResponse(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    _response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    _response = new CurrentResponse();
                    _response.Status = httpResponseMessage.StatusCode;
                    APIErrorResponse apiError = JsonConvert.DeserializeObject<APIErrorResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result); 

                    _response.Message = apiError.Message;
                }
            }
            catch(Exception exc)
            {
                _response = new CurrentResponse();
                _response.Status = HttpStatusCode.BadRequest;
                _response.Message = "Something went Wrong!, Please try again later";
            }
        }
    }
}
