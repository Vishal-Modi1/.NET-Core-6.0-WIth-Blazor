using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using DataModels.VM.User;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Pages.User
{
    partial class InviteUser
    {
        [Parameter] public InviteUserVM inviteUserVM { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        public async Task Invite()
        {
            isBusySubmitButton = true;

            inviteUserVM.ActivationLink = NavigationManager.BaseUri + "Registration?Token=";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.IsEmailExistAsync(dependecyParams, inviteUserVM.Email);
            bool isEmailExist = ManageIsEmailExistResponse(response);

            if(isEmailExist)
            {
                inviteUserVM.ActivationLink = NavigationManager.BaseUri + "InvitationConfirmation?Token=";
            }

            response = await InviteUserService.SaveandUpdateAsync(dependecyParams, inviteUserVM);

            ManageInviteUserResponse(response);

            isBusySubmitButton = true;
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
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        private bool IsValidInvite()
        {
            bool isValidInvite = false;

            return isValidInvite;
        }
    }
}
