using DataModels.VM.Common;
using DataModels.VM.User;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Radzen;
using System.Security.Claims;

namespace FSM.Blazor.Pages.Account
{
    partial class InvitationConfirmation
    {
        string  link;
        bool isValidToken, showError;
        UserVM userVM = new UserVM();
        NotificationMessage message;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StringValues url;

                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out url);

                if (url.Count() > 0 && url[0] != "")
                {
                    this.link = url[0];
                    message = new NotificationMessage().Build(NotificationSeverity.Info, "", "Validating token ...");
                    NotificationService.Notify(message);

                    StateHasChanged();

                    DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                    CurrentResponse response = await AccountService.ValidateTokenAsync(dependecyParams, this.link);
                    await ManageResponseAsync(response);
                    showError = false;
                }
            }
        }

        public async Task AcceptInvitation()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InviteUserService.AcceptInvitationAsync(dependecyParams, this.link);

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", "Something went Wrong!, Please try again later or contact to our administrator department.");
                NotificationService.Notify(message);

                isValidToken = false;
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/Login");
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                NotificationService.Notify(message); 
                StateHasChanged();
            }
        }

        private async Task ManageResponseAsync(CurrentResponse response)
        {
            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", "Token is not valid. Please try with valid token!");
                NotificationService.Notify(message);

                isValidToken = false;
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                    userVM = await UserService.GetMasterDetailsAsync(dependecyParams, true, link);

                    isValidToken = true;

                    message = new NotificationMessage().Build(NotificationSeverity.Success, "", response.Message);
                    NotificationService.Notify(message);
                    StateHasChanged();
                }
                else
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                    NotificationService.Notify(message);
                    isValidToken = false;
                    StateHasChanged();
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                NotificationService.Notify(message);

                isValidToken = false;
                StateHasChanged();
            }
        }
    }
}
