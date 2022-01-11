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

        string submitButtonText = "Reset Password";

        [Inject]
        NotificationService NotificationService { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        public string alertMessageText = "";
        public bool isDisplayAlert = false;
        BadgeStyle alertBadgeStyle;

        bool isDisableResetButton = true, isValidToken;
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isBusy = false;
        string busyText = "";

        ResetPasswordVM resetPasswordVM { get; set; }

        protected override async void OnInitialized()
        {
            base.OnInitialized();

            resetPasswordVM = new ResetPasswordVM();

            StringValues link;

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out link);

            if (link.Count() == 0 || string.IsNullOrWhiteSpace(link[0]))
            {
                DisplayAlert(BadgeStyle.Danger, "Token is not exist! Please try with valid token link.");

                StateHasChanged();
            }
            else
            {
                resetPasswordVM.Token = link[0];
                DisplayAlert(BadgeStyle.Info, "Validating Token..");

                StateHasChanged();
                CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(_httpClient, link[0]);
               
                ManageResponse(response);
            }

        }

        private void ManageResponse(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isDisableResetButton = false;
                    isDisplayAlert = false;
                    isValidToken = true;

                    StateHasChanged();
                }
                else
                {
                    DisplayAlert(BadgeStyle.Danger, "Token is not exist! Please try with valid token link.");

                    StateHasChanged();
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

            StateHasChanged();

            CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(_httpClient, resetPasswordVM.Token);

            ManageResponse(response);

            if (isValidToken)
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
            alertBadgeStyle = badgeStyle;
            alertMessageText = message;
            isDisplayAlert = true;

        }
        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
