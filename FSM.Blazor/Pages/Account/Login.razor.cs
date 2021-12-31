using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;
using System.Security.Claims;

namespace FSM.Blazor.Pages.Account
{
    public partial class Login 
    {
        async Task OnLoginAsync(LoginArgs args, string name)
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignIn", args.Username, args.Password, "/");
        }

        void OnRegister(string name)
        {
            //  console.Log($"{name} -> Register");
        }

        void OnResetPassword(string value, string name)
        {
            NavigationManager.NavigateTo("/forgetpassword");
        }

        private async void btnLogout_Click()
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "./js/auth.js");
            await authModule.InvokeVoidAsync("SignOut", "/");
        }
    }
}
