﻿using DataModels.VM.Account;
using DataModels.VM.Common;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Web.UI.Shared;

namespace Web.UI.Pages.Account
{
    partial class ForgetPassword
    {
        bool isBusy; 
        ForgotPasswordVM forgotPasswordVM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            forgotPasswordVM = new ForgotPasswordVM();
            base.OnInitialized();
        }

        public async Task Submit()
        {
            isBusy = true;

            forgotPasswordVM.ResetURL = NavigationManager.BaseUri + "/ResetPassword?Token=";

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AccountService.ForgetPasswordAsync(dependecyParams, forgotPasswordVM);
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            forgotPasswordVM.Email = "";

            isBusy = false;
        }
    }
}
