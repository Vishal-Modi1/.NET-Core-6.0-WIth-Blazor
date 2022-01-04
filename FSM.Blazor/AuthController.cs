using DataModels.Constants;
using DataModels.VM.Account;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace FSM.Blazor
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpCaller _httpCaller;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpCaller = new HttpCaller();
            _httpClientFactory = httpClientFactory;
        }

        private static readonly AuthenticationProperties COOKIE_EXPIRES = new AuthenticationProperties()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true,
        };

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<ActionResult> SignInPost(LoginVM loginVM)
        {
            string jsonData = JsonConvert.SerializeObject(loginVM);

            var response = await _httpCaller.PostAsync( _httpClientFactory, "Account/login", jsonData);

            if (response.Status == HttpStatusCode.OK)
            {
                await AddCookieAsync(response.Data);
                return this.StatusCode((int)HttpStatusCode.OK);
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet]
        [Route("api/auth/signout")]
        public async Task<ActionResult> SignOutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.Ok();
        }

        [HttpGet]
        [Route("api/auth/signoutget")]
        public async Task<ActionResult> SignOutGet()
        {
            var data = HttpContext.User;

            return this.Ok();
        }

        private async Task AddCookieAsync(string response)
        {
            try
            {
                LoginResponseVM loginResponse = JsonConvert.DeserializeObject<LoginResponseVM>(response);

                var userClaims = new List<Claim>()
             {
                  new Claim(ClaimTypes.Name, loginResponse.FirstName),
                  new Claim(CustomClaimTypes.FullName, loginResponse.FirstName + " " + loginResponse.LastName),
                  new Claim(ClaimTypes.Email, loginResponse.Email),
                  new Claim(CustomClaimTypes.AccessToken, loginResponse.AccessToken),
                  new Claim(CustomClaimTypes.UserId, loginResponse.Id.ToString()),
                  new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(loginResponse.RoleId)),
                  new Claim(CustomClaimTypes.CompanyName, JsonConvert.SerializeObject(loginResponse.CompanyName)),
                  new Claim(CustomClaimTypes.CompanyId, JsonConvert.SerializeObject(loginResponse.CompanyId))
             };

                CurrentUserPermissionManager.AddInCache(loginResponse.Id, loginResponse.UserPermissionList);

                //var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var grandmaIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                Thread.CurrentPrincipal = userPrincipal;

                var authProperties = COOKIE_EXPIRES;

                await HttpContext.SignInAsync(new ClaimsPrincipal(userPrincipal));
            }
            catch (Exception ex)
            {

            }
        }

        private async Task<CurrentResponse> PostAsync(string url, string jsonData)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "");

                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient("FSMAPI");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);
                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);

                return response;
            }
            catch (Exception exc)
            {
                return null;
            }
        }
    }
}
