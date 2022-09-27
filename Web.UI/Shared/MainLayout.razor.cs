using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;

//TODO : UI Pending
//using Web.UI.Pages.MyAccount;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Web.UI.Shared
{
    public partial class MainLayout
    {
        UINotification UINotification { get; set; } = new UINotification();

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        bool sidebarExpanded = true, bodyExpanded = false, isDisplayPopup = false;
        string userFullName = "", loggedUserId, popupTitle = "";
        OperationType operationType;
        List<DropDownValues> companyList = new List<DropDownValues>();
        public int companyId;
        string companyName;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                var user = (await AuthStat).User;
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

                if (!user.Identity.IsAuthenticated)
                {
                        NavigationManager.NavigateTo("/Login");
                }

                userFullName = user.Claims.Where(c => c.Type == CustomClaimTypes.FullName)
                                   .Select(c => c.Value).SingleOrDefault();

                loggedUserId = user.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                                   .Select(c => c.Value).SingleOrDefault();

                companyId = Convert.ToInt32(user.Claims.Where(c => c.Type == CustomClaimTypes.CompanyId)
                                   .Select(c => c.Value).SingleOrDefault());

                companyName = user.Claims.Where(c => c.Type == CustomClaimTypes.CompanyName)
                                   .Select(c => c.Value).SingleOrDefault();

                if (string.IsNullOrEmpty(companyName))
                {
                    companyName = "Flight Schedule Management";
                }

                base.StateHasChanged();
            }
        }

        async Task OpenChangePasswordDailog()
        {
            isDisplayPopup = true;
            popupTitle = "Change Password";
            operationType = OperationType.ChangePassword;

            // TODO
            //await DialogService.OpenAsync<ChangePassword>("Change Password",
            //     new Dictionary<string, object>() { { "Id", loggedUserId } },  new DialogOptions() { Width = "500px", Height = "380px" });
        }

        async Task OpenChangeCompanyDailog()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyList = await CompanyService.ListDropDownValuesByUserId(dependecyParams,Convert.ToInt64(loggedUserId));
            
            isDisplayPopup = true;
            popupTitle = "Change Company";
            operationType = OperationType.ChangeCompany;

            // TODO
            //await DialogService.OpenAsync<ChangePassword>("Change Password",
            //     new Dictionary<string, object>() { { "Id", loggedUserId } },  new DialogOptions() { Width = "500px", Height = "380px" });
        }

        async Task CloseDialog()
        {
            isDisplayPopup = false;

        }

        // TODO
        //async Task ManageMenuClickEvent(MenuItemEventArgs eventArgs)
        //{
        //    if (eventArgs.Text == "Change Password")
        //    {
        //       await OpenChangePasswordDailog();
        //    }
        //}

        public string GetClaimValue(string claimType)
        {
            if (AuthenticationStateProvider == null)
            {
                return "";
            }

            ClaimsPrincipal cp = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();

            return claimValue;
        }
    }
}
