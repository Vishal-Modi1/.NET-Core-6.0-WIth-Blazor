using DataModels.VM.Account;
using DataModels.VM.Common;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FSM.Blazor.Pages.Account
{
    partial class ForgetPassword
    {
        bool isBusy;
        
        string submitButtonText = "Submit";

        ForgotPasswordVM ForgotPasswordVM { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        protected override async Task OnInitializedAsync()
        {
            ForgotPasswordVM = new ForgotPasswordVM();
            base.OnInitialized();
        }

        public async Task Submit()
        {
            SetButtonState(true);

            ForgotPasswordVM.ResetURL = NavigationManager.BaseUri + "/ResetPassword?Token=";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await AccountService.ForgetPasswordAsync(dependecyParams, ForgotPasswordVM);

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
