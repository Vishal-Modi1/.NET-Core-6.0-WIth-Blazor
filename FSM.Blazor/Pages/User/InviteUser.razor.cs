using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using DataModels.VM.User;
using FSM.Blazor.Utilities;
using Radzen;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using DataModels.Enums;

namespace FSM.Blazor.Pages.User
{
    partial class InviteUser
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        [Parameter] public InviteUserVM InviteUserVM { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public bool isBusy = false, isSuperAdmin = false;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthStat).User;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            isSuperAdmin = Convert.ToInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role)
                             .Select(c => c.Value).SingleOrDefault()) == (int)UserRole.SuperAdmin;

        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        public async Task Invite()
        {
            isBusy = true;

            InviteUserVM.ActivationLink = NavigationManager.BaseUri + "Registration?Token=";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.IsEmailExistAsync(dependecyParams, InviteUserVM.Email);
            bool isEmailExist = ManageIsEmailExistResponse(response);

            if(isEmailExist)
            {
                InviteUserVM.ActivationLink = NavigationManager.BaseUri + "InvitationConfirmation?Token=";
            }

            response = await InviteUserService.SaveandUpdateAsync(dependecyParams, InviteUserVM);

            ManageInviteUserResponse(response);

            isBusy = false;
            this.StateHasChanged();
        }

        private bool ManageIsEmailExistResponse(CurrentResponse response)
        {
            bool isEmailExist = true;

            if (response != null && response.Status == System.Net.HttpStatusCode.OK && Convert.ToBoolean(response.Data)  == false)
            {
                isEmailExist = false;
            }

            return isEmailExist;
        }

        private void ManageInviteUserResponse(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.Ambiguous)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Invite User", response.Message);
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Invite User", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Invite User", response.Message);
                NotificationService.Notify(message);
            }
        }

        private bool IsValidInvite()
        {
            bool isValidInvite = false;

            return isValidInvite;
        }

    }
}
