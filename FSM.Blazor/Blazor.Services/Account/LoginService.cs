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

    }
}
