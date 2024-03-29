﻿using DataModels.VM.Common;
using DataModels.VM.User;
using Web.UI.Extensions;
using Web.UI.Utilities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Web.UI.Pages.Account
{
    partial class InvitationConfirmation
    {
        string link;
        UserVM userVM = new UserVM();

        protected override Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StringValues url;

                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out url);

                if (url.Count() == 0 || url[0] == "")
                {
                    globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
                }
                else
                {
                    this.link = url[0];
                    dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                    CurrentResponse response = await AccountService.ValidateTokenAsync(dependecyParams, this.link);
                    await ManageResponseAsync(response);
                }

                ChangeLoaderVisibilityAction(false);
                base.StateHasChanged();
            }
        }

        public async Task AcceptInvitation()
        {
            isBusySubmitButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InviteUserService.AcceptInvitationAsync(dependecyParams, this.link);

            isBusySubmitButton = false;

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/Login");
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }
        }

        private async Task ManageResponseAsync(CurrentResponse response)
        {
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                userVM = await UserService.GetMasterDetailsAsync(dependecyParams, true, link);
            }
            else
            {
                NavigationManager.NavigateTo("/TokenExpired");
            }
        }
    }
}
