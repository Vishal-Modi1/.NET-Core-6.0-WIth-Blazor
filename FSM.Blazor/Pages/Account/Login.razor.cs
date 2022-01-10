﻿using DataModels.VM.Account;
using Microsoft.JSInterop;
using Radzen;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.Account
{
    public partial class Login 
    {

        public LoginVM loginVM = new LoginVM();
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        private string? result;
        private DotNetObjectReference<Login>? objRef;

        async Task Submit()
        {
            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("SignIn", loginVM.Email, loginVM.Password, "/");

            objRef = DotNetObjectReference.Create(this);
            result = await authModule.InvokeAsync<string>("ManageLoginResponse", objRef, "");
        }

        [JSInvokable]
        public void ManageLoginResponse(string responseMessage)
        {
            NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Error, responseMessage, "");
            NotificationService.Notify(message); ;
        }
    }
}
