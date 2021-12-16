using DataModels.Constants;
using DataModels.VM.Account;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace FSM.Blazor
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpCaller _httpCaller;

        public AuthController(IHttpContextAccessor httpContextAccessor)
        {
            _httpCaller = new HttpCaller();
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
            //CurrentResponse response = await _httpCaller.PostAsync("Account/login", jsonData);

            //if (response.Status == System.Net.HttpStatusCode.OK)
            //{
            //    await AddCookieAsync(response.Data);
            //    return this.StatusCode((int)HttpStatusCode.OK);
            //}
            //else
            //{
            //    return this.StatusCode((int)HttpStatusCode.Unauthorized);
            //}

            return null;
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

                await HttpContext.SignInAsync( new ClaimsPrincipal(userPrincipal));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
