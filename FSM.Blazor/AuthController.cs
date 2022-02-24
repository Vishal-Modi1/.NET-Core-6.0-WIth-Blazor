using DataModels.Constants;
using DataModels.VM.Account;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly CurrentUserPermissionManager _currentUserPermissionManager;
        private readonly IHttpClientFactory _httpClient;

        public AuthController(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClient, IMemoryCache memoryCache)
        {
            _httpCaller = new HttpCaller();
            _httpClient = httpClient;
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
        }

        private static readonly AuthenticationProperties COOKIE_EXPIRES = new AuthenticationProperties()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true,
        };

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<IActionResult> SignInPost(LoginVM loginVM)
        {
            string jsonData = JsonConvert.SerializeObject(loginVM);

            var response = await _httpCaller.PostAsync( _httpClient, "Account/login", jsonData);

            jsonData = JsonConvert.SerializeObject(response.Data);

            if (response.Status == HttpStatusCode.OK)
            {
                await AddCookieAsync(response.Data);
            }

            return Ok(response);
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

        private async Task AddCookieAsync(object response)
        {
            try
            {
                LoginResponseVM loginResponse = JsonConvert.DeserializeObject<LoginResponseVM>(response.ToString());

                var userClaims = new List<Claim>()
                {
                  new Claim(ClaimTypes.Name, loginResponse.FirstName),
                  new Claim(CustomClaimTypes.FullName, loginResponse.FirstName + " " + loginResponse.LastName),
                  new Claim(ClaimTypes.Email, loginResponse.Email),
                  new Claim(CustomClaimTypes.AccessToken, loginResponse.AccessToken),
                  new Claim(CustomClaimTypes.UserId, loginResponse.Id.ToString()),
                  new Claim(ClaimTypes.Role, loginResponse.RoleId.ToString()),
                  new Claim(CustomClaimTypes.CompanyName, loginResponse.CompanyName == null ? "" : loginResponse.CompanyName),
                  new Claim(CustomClaimTypes.CompanyId, loginResponse.CompanyId.ToString()),
                  new Claim(CustomClaimTypes.ProfileImageURL, loginResponse.ImageURL),
                  new Claim(CustomClaimTypes.TimeZone, loginResponse.LocalTimeZone)
               };

                _currentUserPermissionManager.AddInCache(loginResponse.Id, loginResponse.UserPermissionList);

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

                var client = _httpClient.CreateClient("FSMAPI");
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
