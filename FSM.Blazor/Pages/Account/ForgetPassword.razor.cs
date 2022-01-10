using DataModels.VM.Account;
using DataModels.VM.Common;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FSM.Blazor.Pages.Account
{
    partial class ForgetPassword
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        bool isBusy;

        [Inject]
        NotificationService NotificationService { get; set; }

        ForgotPasswordVM ForgotPasswordVM { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        protected override void OnInitialized()
        {
            ForgotPasswordVM = new ForgotPasswordVM();
            base.OnInitialized();
        }

        public async Task Submit()
        {
            SetButtonState(true);

            ForgotPasswordVM.ResetURL = NavigationManager.BaseUri + "/ResetPassword?Token=";

            CurrentResponse response = await AccountService.ForgetPasswordAsync(_httpClient, ForgotPasswordVM);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Forget Password", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Forget Password", response.Message);
                NotificationService.Notify(message);
            }

            SetButtonState(false);
        }

        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
