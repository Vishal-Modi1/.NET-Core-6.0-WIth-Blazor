using DataModels.Constants;
using DataModels.VM.Account;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FSM.Blazor.Blazor.Services.Account
{
    public class LoginService
    {
        private readonly HttpCaller _httpCaller;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
            _httpContextAccessor = httpContextAccessor;
        }

        public async void LoginAsync(string userName, string password)
        {
            //var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", new object[] { ".//js//auth.js" });
            //await authModule.InvokeVoidAsync("SignIn", "denis@voituron.net", "MyPassword", "/");
        }

        private async Task AddCookieAsync(string response)
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

            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            Thread.CurrentPrincipal = userPrincipal;

//            await _httpContextAccessor.HttpContext.Response.Cookies.Append(userPrincipal);

        }
    }
}
