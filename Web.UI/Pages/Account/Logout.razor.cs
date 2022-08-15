using DataModels.Constants;
using Microsoft.JSInterop;

namespace Web.UI.Pages.Account
{
    public partial class Logout
    {
        protected override async Task OnInitializedAsync()
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignOut", "/");

            var cp = (await AuthStat).User;

            string loggedUserId = cp.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                               .Select(c => c.Value).SingleOrDefault();


            MemoryCache.Remove(Convert.ToInt64(loggedUserId));
        }
    }
}
