using DataModels.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;

namespace FSM.Blazor.Pages.Account
{
    public partial class Logout
    {

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignOut", "/");

            var cp = (await AuthStat).User;

            string loggedUserId = cp.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                               .Select(c => c.Value).SingleOrDefault();


            MemoryCache.Remove(Convert.ToInt32(loggedUserId));
        }
    }
}
