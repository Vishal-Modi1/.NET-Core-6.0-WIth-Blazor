using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace FSM.Blazor.Pages.Account
{
    public partial class Logout
    {
        protected override async Task OnInitializedAsync()
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignOut", "/");
        }
    }
}
