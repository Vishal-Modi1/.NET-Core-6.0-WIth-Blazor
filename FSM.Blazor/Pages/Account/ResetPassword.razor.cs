using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using DataModels.VM.Account;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using DataModels.VM.Common;
using Radzen.Blazor;
using FSM.Blazor.Utilities;

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

        NotificationMessage message;
        ResetPasswordVM resetPasswordVM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            resetPasswordVM = new ResetPasswordVM();

            StringValues link;

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out link);

            if (link.Count() == 0 || string.IsNullOrWhiteSpace(link[0]))
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else
            {
                resetPasswordVM.Token = link[0];

                message = new NotificationMessage().Build(NotificationSeverity.Info, "" , "Validating Token..");
                NotificationService.Notify(message);

                DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);

                CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(dependecyParams, link[0]);

                ManageResponse(response);
            }

        }

        private void ManageResponse(CurrentResponse response)
        {

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    //message = new NotificationMessage().Build(NotificationSeverity.Info, "Reset Password", "Password reset successfully!");
                    //NotificationService.Notify(message);
                    isValidToken = true;
                    isDisableResetButton = false;

                    StateHasChanged();

                }
                else
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "", "Token is not exist! Please try with valid token link.");
                    NotificationService.Notify(message);

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
            // DisplayAlert(BadgeStyle.Info, "Validating Token..");

            // StateHasChanged();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(dependecyParams, resetPasswordVM.Token);

            ManageResponse(response);

            if (isValidToken)
            {
                response = await AccountService.ResetPasswordAsync(dependecyParams, resetPasswordVM);

                NotificationMessage message;

                if (response == null)
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                    NotificationService.Notify(message);
                }
                else if (((int)response.Status) == 200)
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Success, "Reset Password", response.Message);
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
