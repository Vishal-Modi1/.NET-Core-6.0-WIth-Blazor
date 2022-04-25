using DataModels.Constants;
using DataModels.Models;
using DataModels.VM.Account;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(5000),
            IsPersistent = true,
        };


        [Authorize]
        [HttpGet]
        [Route("api/auth/test")]
        public async Task<IActionResult> SignInPost1()
        {
            return Ok("");
        }

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<IActionResult> SignInPost(LoginVM loginVM)
        {
            string jsonData = JsonConvert.SerializeObject(loginVM);

            var response = await _httpCaller.PostAsync(_httpClient, "Account/login", jsonData);

            jsonData = JsonConvert.SerializeObject(response.Data);

            if (response.Status == HttpStatusCode.OK)
            {
                await AddCookieAsync(response.Data);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("api/auth/refreshtoken")]
        public async Task<IActionResult> RefreshToken(string refreshToken, long userId)
        {
            string url = $"Account/RefreshToken?refreshToken={refreshToken}&userId={userId}";
            var response = await _httpCaller.GetAsync(_httpClient, url);

            string jsonData = JsonConvert.SerializeObject(response.Data);

            if (response.Status == HttpStatusCode.OK)
            {
                await UpdateCookieAsync(response.Data);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("api/auth/signout")]
        public async Task<ActionResult> SignOutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var user = User as ClaimsPrincipal;
            var identity = user.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var claimNameList = identity.Claims.ToList();
                foreach (var claim in claimNameList)
                {
                    identity.RemoveClaim(claim);
                }
            }

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
                  new Claim(CustomClaimTypes.RefreshToken, loginResponse.RefreshToken),
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

                //var authProperties = COOKIE_EXPIRES;

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(userPrincipal));
            }
            catch (Exception ex)
            {

            }
        }

        private async Task UpdateCookieAsync(object response)
        {
            try
            {
                RefreshTokenModel refreshTokenModel = JsonConvert.DeserializeObject<RefreshTokenModel>(response.ToString());

                // create a new identity from the old one
                var identity = new ClaimsIdentity(User.Identity);

                // update claim value
                //identity.RemoveClaim(identity.FindFirst(CustomClaimTypes.AccessToken));
                //identity.AddClaim(new Claim(CustomClaimTypes.AccessToken, refreshTokenModel.AccessToken));

                //identity.RemoveClaim(identity.FindFirst(CustomClaimTypes.RefreshToken));
                //identity.AddClaim(new Claim(CustomClaimTypes.RefreshToken, refreshTokenModel.RefreshToken));

                var userClaims = new List<Claim>()
                {
                  new Claim(ClaimTypes.Name, identity.FindFirst(ClaimTypes.Name).Value),
                  new Claim(CustomClaimTypes.FullName, identity.FindFirst(CustomClaimTypes.FullName).Value),
                  new Claim(ClaimTypes.Email, identity.FindFirst(CustomClaimTypes.FullName).Value),
                  new Claim(CustomClaimTypes.AccessToken,  refreshTokenModel.AccessToken),
                  new Claim(CustomClaimTypes.RefreshToken, refreshTokenModel.RefreshToken),
                  new Claim(CustomClaimTypes.UserId, identity.FindFirst(CustomClaimTypes.UserId).Value),
                  new Claim(ClaimTypes.Role, identity.FindFirst(ClaimTypes.Role).Value),
                  new Claim(CustomClaimTypes.CompanyName, identity.FindFirst(CustomClaimTypes.CompanyName).Value),
                  new Claim(CustomClaimTypes.CompanyId, identity.FindFirst(CustomClaimTypes.CompanyId).Value),
                  new Claim(CustomClaimTypes.ProfileImageURL, identity.FindFirst(CustomClaimTypes.ProfileImageURL).Value),
                  new Claim(CustomClaimTypes.TimeZone, identity.FindFirst(CustomClaimTypes.TimeZone).Value),
               };

           //     _currentUserPermissionManager.AddInCache(loginResponse.Id, loginResponse.UserPermissionList);

                //var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var grandmaIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                Thread.CurrentPrincipal = userPrincipal;

                //var authProperties = COOKIE_EXPIRES;

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(userPrincipal));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
