﻿using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using DataModels.VM.Account;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using DataModels.VM.Common;
using Web.UI.Utilities;
using Web.UI.Shared;
using Telerik.Blazor.Components;
using static Telerik.Blazor.ThemeConstants;

namespace Web.UI.Pages.Account
{
    partial class ResetPassword
    {
        NotificationModel message;
        CurrentResponse response;

        public string Link { get; set; }

        bool isValidToken;
        bool isBusy = false;

        ResetPasswordVM resetPasswordVM = new ResetPasswordVM();

        protected override Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                resetPasswordVM = new ResetPasswordVM();

                StringValues link;

                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

                QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out link);

                if (link.Count() == 0 || string.IsNullOrWhiteSpace(link[0]))
                {
                    globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
                }
                else
                {
                    isBusy = true;
                    resetPasswordVM.Token = link[0];
                    //globalMembers.UINotification.DisplayInfoNotification(globalMembers.UINotification.Instance, "Validating Token...");

                    dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                    response = await AccountService.ValidateTokenAsync(dependecyParams, link[0]);

                    ManageResponse();
                }

                 ChangeLoaderVisibilityAction(false);
                base.StateHasChanged();
            }

            base.OnAfterRenderAsync(firstRender);
        }

        private void ManageResponse()
        {
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
               // globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, response.Message);

                isBusy = false; isValidToken = true;
                StateHasChanged();
            }
            else
            {
                //globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
                //isBusy = false; isValidToken = false;

                NavigationManager.NavigateTo("/TokenExpired");
            }
        }

        public async Task Submit()
        {
            isBusy = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            response = await AccountService.ResetPasswordAsync(dependecyParams, resetPasswordVM);
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            resetPasswordVM.Password = "";
            resetPasswordVM.ConfirmPassword = "";

            isBusy = false;
        }
    }
}
