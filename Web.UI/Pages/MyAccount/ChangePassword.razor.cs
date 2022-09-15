using DataModels.VM.Common;
using DataModels.VM.MyAccount;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.Constants;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Pages.MyAccount
{
    partial class ChangePassword
    {
        public string id { get; set; }
        private ChangePasswordVM changePasswordVM = new ChangePasswordVM();

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/Login");
            }

            id = user.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                          .Select(c => c.Value).SingleOrDefault();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            changePasswordVM.UserId = Convert.ToInt64(id);
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await MyAccountService.ChangePassword(dependecyParams, changePasswordVM);

            isBusySubmitButton = false;
            StateHasChanged();

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            changePasswordVM = new ChangePasswordVM();
            changePasswordVM.UserId = Convert.ToInt64(id);
        }
    }
}
