using DataModels.VM.Account;
using Microsoft.JSInterop;
using Radzen;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace FSM.Blazor.Pages.Account
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
            SetButtonState(true);

            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignIn", loginVM.Email, loginVM.Password, "/");

            objRef = DotNetObjectReference.Create(this);
            result = await authModule.InvokeAsync<string>("SetDotNetObject", objRef, "");
        }

        [JSInvokable]
        public void ManageLoginResponse(string responseMessage)
        {
            NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Error, responseMessage, "");
            NotificationService.Notify(message);

            SetButtonState(false);
        }

        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
