using DataModels.VM.Account;
using Microsoft.JSInterop;
using Web.UI.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Telerik.Blazor.Components;
using Microsoft.AspNetCore.Components;
using Web.UI.Shared;
using static Telerik.Blazor.ThemeConstants;

namespace Web.UI.Pages.Account
{
    public partial class Login 
    {
        public LoginVM loginVM = new LoginVM();
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        
        bool isBusy;
        
        private string? result;
        private DotNetObjectReference<Login>? objRef;
        bool isSessionTimeout;

        protected override Task OnInitializedAsync()
        {
            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("TokenExpired", out link);

            if (link.Count() > 0 && link[0] == "true")
            {
                isSessionTimeout = true;
            }

            return base.OnInitializedAsync();
        }

        async Task Submit()
        {
            try
            {
                SetButtonState(true);

                var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
                await authModule.InvokeVoidAsync("SignIn", loginVM.Email, loginVM.Password, "/Dashboard");

                objRef = DotNetObjectReference.Create(this);
                result = await authModule.InvokeAsync<string>("SetDotNetObject", objRef, "");

                this.StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        [JSInvokable]
        public void ManageLoginResponse(string responseMessage)
        {
            uiNotification.DisplayCustomErrorNotification(uiNotification.Instance, responseMessage);
            SetButtonState(false);
        }

        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
