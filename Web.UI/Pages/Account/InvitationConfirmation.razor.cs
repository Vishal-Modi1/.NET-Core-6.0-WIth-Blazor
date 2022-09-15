using DataModels.VM.Common;
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
            isDisplayLoader = true;
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
                    uiNotification.DisplayErrorNotification(uiNotification.Instance);
                }
                else
                {
                    this.link = url[0];
                    DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                    CurrentResponse response = await AccountService.ValidateTokenAsync(dependecyParams, this.link);
                    await ManageResponseAsync(response);
                }

                isDisplayLoader = false;
                base.StateHasChanged();
            }
        }

        public async Task AcceptInvitation()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InviteUserService.AcceptInvitationAsync(dependecyParams, this.link);

            isBusySubmitButton = false;

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/Login");
            }
            else
            {
                uiNotification.DisplayCustomErrorNotification(uiNotification.Instance, response.Message);
            }
        }

        private async Task ManageResponseAsync(CurrentResponse response)
        {
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                userVM = await UserService.GetMasterDetailsAsync(dependecyParams, true, link);
            }
            else
            {
                NavigationManager.NavigateTo("/TokenExpired");
            }
        }
    }
}
