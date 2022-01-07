using DataModels.VM.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;
using System.Security.Claims;

namespace FSM.Blazor.Pages.Account
{
    public partial class Login 
    {

        public LoginVM loginVM = new LoginVM();
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        async Task Submit()
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignIn", loginVM.Email, loginVM.Password, "/");
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            //if (firstRender)
            //{
            //    // See warning about memory above in the article
            //    var dotNetReference = DotNetObjectReference.Create(this);
            //    var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");

            //    await authModule.InvokeVoidAsync("BlazorUniversity.startRandomGenerator", dotNetReference);
            //}
        }


        [JSInvokable("AddText")]
        public void LoginFailed(string text)
        {
           
        }
    }
}
