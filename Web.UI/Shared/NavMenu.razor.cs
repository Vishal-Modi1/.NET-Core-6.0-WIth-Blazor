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

namespace Web.UI.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        UINotification UINotification { get; set; } = new UINotification();

        [Parameter] public bool Expanded { get; set; }
        [Parameter] public bool IsMainContainerTransparent { get; set; } = true;

        bool sidebarExpanded = true, bodyExpanded = false, isSuperAdmin;

        string fullName = "", profileImageURL = "";

        List<MenuItem> menuItems;
        TelerikDrawer<MenuItem> DrawerRef { get; set; }
        MenuItem SelectedItem { get; set; }
        bool isAdministrationTabAdded = false;
        bool DrawerExpanded { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null &&
                 httpContextAccessor.HttpContext.Request != null && httpContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                var userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
                if (!string.IsNullOrEmpty(userAgent))
                {
                    if (userAgent.Contains("iPhone") || userAgent.Contains("Android") || userAgent.Contains("Googlebot"))
                    {
                        sidebarExpanded = false;
                        bodyExpanded = true;
                    }
                }
            }

            isAdministrationTabAdded = false;

            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/Login");
            }

            if (user.Identity.IsAuthenticated)
            {
                fullName = user.Claims.Where(c => c.Type == CustomClaimTypes.FullName)
                          .Select(c => c.Value).SingleOrDefault();

                profileImageURL = user.Claims.Where(c => c.Type == CustomClaimTypes.ProfileImageURL)
                           .Select(c => c.Value).SingleOrDefault();

                isSuperAdmin = Convert.ToUInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value) == (int)UserRole.SuperAdmin;


                menuItems = new List<MenuItem>();
                menuItems.Add(new MenuItem() { Controller = "Dashboard", DisplayName = "Dashboard", FavIconStyle = "group" });

                if (isSuperAdmin)
                {
                    menuItems.Add(new MenuItem() { Controller = "Administration", DisplayName = "Administration", });
                }

                menuItems.AddRange(await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider));
                menuItems.Add(new MenuItem() { Controller = "Logout", DisplayName = "Log out", FavIconStyle = "group" });

                string currPage = NavigationManager.Uri;
                MenuItem ActivePage = menuItems.FirstOrDefault();

                if (ActivePage != null)
                {
                    SelectedItem = ActivePage;
                }
            }
        }

        public async Task NavigateToPageAsync(MenuItem item)
        {
            try
            {
                SelectedItem = item;

                if (item.Controller.ToLower() == Module.Company.ToString().ToLower())
                {
                    await OpenCompanyDetailPage(1);
                }
                else
                {
                    NavigationManager.NavigateTo("/" + SelectedItem.Controller);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string GetSelectedItemClass(MenuItem item)
        {
            if (SelectedItem == null)
            {
                return string.Empty;
            }
            return SelectedItem.DisplayName.ToLowerInvariant().Equals(item.DisplayName.ToLowerInvariant()) ? "selected-nav-item" : "";
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
}
