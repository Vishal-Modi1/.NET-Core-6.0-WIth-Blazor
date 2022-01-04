﻿using DataModels.VM.Common;
using DataModels.VM.MyAccount;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FSM.Blazor.Pages.MyAccount
{
    partial class ChangePassword
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private ChangePasswordVM changePasswordVM = new ChangePasswordVM();

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        public async Task Submit()
        {
            changePasswordVM.UserId = Convert.ToInt32(Id);
            CurrentResponse response = await MyAccountService.ChangePassword(_httpClient, changePasswordVM);

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
    }
}
