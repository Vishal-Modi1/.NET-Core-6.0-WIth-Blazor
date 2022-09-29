using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using Web.UI.Data.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components;
using Web.UI.Utilities;
using System.Text;
using DataModels.VM.Company;
using Web.UI.Models.Shared;

namespace Web.UI.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        UINotification UINotification { get; set; } = new UINotification();
        public bool isDisplayPageLoader { get; set; }

        [Parameter] public bool Expanded { get; set; }
        [Parameter] public bool IsMainContainerTransparent { get; set; } = true;

        public GlobalMembers globalMembers = new GlobalMembers();

        TelerikDrawer<MenuItem> drawerRef { get; set; }
        
        bool isDrawerExpanded { get; set; } = true;
        public NavigationHeaderModel navigationHeaderModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/Login");
            }

            if (user.Identity.IsAuthenticated)
            {
                InitializeGlobalMembers(user);

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                await SetNavigationHeaderValues(user, dependecyParams);

                await SetMenuItems();
            }
        }

        private async Task SetMenuItems()
        {
            globalMembers.MenuItems = new List<MenuItem>();
            globalMembers.MenuItems.Add(new MenuItem() { Controller = "Dashboard", Name = "Dashboard", DisplayName = "Dashboard", FavIconStyle = "group" });

            if (navigationHeaderModel.IsSuperAdmin)
            {
                globalMembers.MenuItems.Add(new MenuItem() { Controller = "Administration", Name = "Administration", DisplayName = "Administration", });
            }

            globalMembers.MenuItems.AddRange(await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider));
            globalMembers.MenuItems.Add(new MenuItem() { Controller = "Logout", DisplayName = "Log out", FavIconStyle = "group" });

            string currPage = NavigationManager.Uri;
            MenuItem ActivePage = globalMembers.MenuItems.FirstOrDefault();
        }

        private void InitializeGlobalMembers(ClaimsPrincipal user)
        {
            globalMembers.UINotification = UINotification;
            globalMembers.UserRole = (UserRole)(Convert.ToInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value));
            globalMembers.IsSuperAdmin = Convert.ToUInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value) == (int)UserRole.SuperAdmin;
            globalMembers.IsAdmin = Convert.ToUInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value) == (int)UserRole.Admin;
        }

        private async Task SetNavigationHeaderValues(ClaimsPrincipal user, DependecyParams dependecyParams)
        {
            string loggedUserId = user.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
               .Select(c => c.Value).SingleOrDefault();

            navigationHeaderModel = new NavigationHeaderModel();
            navigationHeaderModel.User = new DataModels.VM.User.UserVM();
            navigationHeaderModel.Company = new CompanyVM();
            navigationHeaderModel.CompanyList = new List<DropDownValues>();

            navigationHeaderModel.User.Id = Convert.ToInt64(loggedUserId);

            navigationHeaderModel.CompanyList = await CompanyService.ListDropDownValuesByUserId(dependecyParams, Convert.ToInt64(loggedUserId));

            navigationHeaderModel.User.FirstName = user.Claims.Where(c => c.Type == CustomClaimTypes.FullName)
                      .Select(c => c.Value).SingleOrDefault();

            navigationHeaderModel.Company.Name = user.Claims.Where(c => c.Type == CustomClaimTypes.CompanyName)
                     .Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(navigationHeaderModel.Company.Name))
            {
                navigationHeaderModel.Company.Name = "Flight Schedule Management";
            }

            navigationHeaderModel.User.ImageName = user.Claims.Where(c => c.Type == CustomClaimTypes.ProfileImageURL)
                       .Select(c => c.Value).SingleOrDefault();

            navigationHeaderModel.IsSuperAdmin = Convert.ToUInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value) == (int)UserRole.SuperAdmin;

        }

        public void ChangeLoaderVisibility(bool visible)
        {
            isDisplayPageLoader = visible;
            StateHasChanged();
        }

        public async Task NavigateToPageAsync(MenuItem item)
        {
            try
            {
                globalMembers.SelectedItem = item;

                if (item.Controller.ToLower() == Module.Company.ToString().ToLower())
                {
                    await OpenCompanyDetailPage(1);
                }
                else
                {
                    NavigationManager.NavigateTo("/" + globalMembers.SelectedItem.Controller);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string GetSelectedItemClass(MenuItem item)
        {
            if (globalMembers.SelectedItem == null)
            {
                return string.Empty;
            }
            return globalMembers.SelectedItem.DisplayName.ToLowerInvariant().Equals(item.DisplayName.ToLowerInvariant()) ? "selected-nav-item" : "";
        }

        async Task OpenCompanyDetailPage(int id)
        {
            CompanyVM companyData = new CompanyVM();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + "FlyManager");
            var data = Encoding.Default.GetBytes(id.ToString());
            NavigationManager.NavigateTo("CompanyDetails?CompanyId=" + System.Convert.ToBase64String(encodedBytes));
        }
    }

    public class GlobalMembers
    {
        public UINotification UINotification { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        public MenuItem SelectedItem { get; set; }

        public UserRole UserRole { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }
    }
}
