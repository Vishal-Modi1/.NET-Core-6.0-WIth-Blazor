using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using DataModels.VM.Account;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using DataModels.VM.Common;
using Radzen.Blazor;

namespace FSM.Blazor.Pages.Account
{
    partial class ResetPassword
    {
        //[Parameter]
        public string Link { get; set; }

        bool isBusy;

        string submitButtonText = "Reset Password";

        [Inject]
        NotificationService NotificationService { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        RadzenBadge alertMessage;

        bool isDisableResetButton = true, isValidToken;
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        
        ResetPasswordVM resetPasswordVM { get; set; }

        protected override async void OnInitialized()
        {
            alertMessage = new RadzenBadge();
            resetPasswordVM = new ResetPasswordVM();

            StringValues link;

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out link);

            if (link.Count() == 0 ||  string.IsNullOrWhiteSpace(link[0]))
            {
                DisplayAlert(BadgeStyle.Danger, "Token is not exist! Please try with valid token link.");
            }
            else
            {
                resetPasswordVM.Token = link[0];
                DisplayAlert(BadgeStyle.Info, "Validating Token..");
                CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(_httpClient, link[0]);
                ManageResponse(response);
            }

            base.OnInitialized();
        }

        private void ManageResponse(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isDisableResetButton = false;
                    alertMessage.Visible = false;
                    isValidToken = true;

                    base.StateHasChanged();
                 }
                else
                {
                    DisplayAlert(BadgeStyle.Danger, "Token is not exist! Please try with valid token link.");
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Reset Password", response.Message);
                NotificationService.Notify(message);
            }
        }

        public async Task Submit(ResetPasswordVM resetPasswordVM)
        {
            isValidToken = false;
            DisplayAlert(BadgeStyle.Info, "Validating Token..");

            CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(_httpClient, resetPasswordVM.Token);

            ManageResponse(response);

            if(isValidToken)
            {
                response = await AccountService.ResetPasswordAsync(_httpClient, resetPasswordVM);

                NotificationMessage message;

                if (response == null)
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                    NotificationService.Notify(message);
                }
                else if (((int)response.Status) == 200)
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Reset Password", "Successfuly done.");
                    NotificationService.Notify(message);
                }
                else
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Reset Password", response.Message);
                    NotificationService.Notify(message);
                }
            }
        }

        private void DisplayAlert(BadgeStyle badgeStyle, string message)
        {
            alertMessage.BadgeStyle = badgeStyle;
            alertMessage.Text = message;
            alertMessage.Visible = true;
        }
        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
