using DataModels.VM.Account;
using Microsoft.JSInterop;
using Web.UI.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Telerik.Blazor.Components;
using Web.UI.Models.Enums;
using Microsoft.AspNetCore.Components;
using Web.UI.Shared;

namespace Web.UI.Pages.Account
{
    public partial class Login 
    {
        [CascadingParameter] protected Notification Notification { get; set; }

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
                await authModule.InvokeVoidAsync("SignIn", loginVM.Email, loginVM.Password, "/");

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
            NotificationModel message;

            message = new NotificationModel().Build(TelerikNotificationTypes.info, "Something went Wrong!, Please try again later.");
            Notification.Instance.Show(message);

            SetButtonState(false);
        }

        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
