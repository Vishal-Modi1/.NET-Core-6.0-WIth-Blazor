using DataModels.VM.Common;
using DataModels.VM.MyAccount;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FSM.Blazor.Pages.MyAccount
{
    partial class ChangePassword
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        private ChangePasswordVM changePasswordVM = new ChangePasswordVM();

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isBusy = false;

        public async Task Submit()
        {
            isBusy = true;
            StateHasChanged();

            changePasswordVM.UserId = Convert.ToInt32(Id);
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await MyAccountService.ChangePassword(dependecyParams, changePasswordVM);

            isBusy = false;
            StateHasChanged();

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Change Password", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Change Password", response.Message);
                NotificationService.Notify(message);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
