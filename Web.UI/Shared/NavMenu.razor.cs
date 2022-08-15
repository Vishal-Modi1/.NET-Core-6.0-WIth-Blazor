using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using Web.UI.Data.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components;

namespace Web.UI.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        UINotification UINotification { get; set; } = new UINotification();

        [Parameter] public bool Expanded { get; set; }

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
                menuItems = new List<MenuItem>();
                menuItems.Add(new MenuItem() { Controller = "Dashboard", DisplayName = "Dashboard", FavIconStyle = "group" });
                menuItems.Add(new MenuItem() { Controller = "Administration", DisplayName = "Administration"});
                menuItems.AddRange(await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider));

                menuItems.Add(new MenuItem() { Controller = "Logout", DisplayName = "Log out", FavIconStyle = "group" });

                fullName = user.Claims.Where(c => c.Type == CustomClaimTypes.FullName)
                          .Select(c => c.Value).SingleOrDefault();

                profileImageURL = user.Claims.Where(c => c.Type == CustomClaimTypes.ProfileImageURL)
                           .Select(c => c.Value).SingleOrDefault();

                isSuperAdmin = Convert.ToUInt32(user.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value) == (int)UserRole.SuperAdmin;

                string currPage = NavigationManager.Uri;
                MenuItem ActivePage = menuItems.FirstOrDefault();

                if (ActivePage != null)
                {
                    SelectedItem = ActivePage;
                }
            }
        }

        public void NavigateToPage(MenuItem item)
        {
            try
            {
                SelectedItem = item;
                NavigationManager.NavigateTo("/" + SelectedItem.Controller);

                base.StateHasChanged();
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
    }
}
