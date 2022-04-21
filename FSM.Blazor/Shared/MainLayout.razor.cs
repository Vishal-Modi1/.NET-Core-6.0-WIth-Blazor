using DataModels.Constants;
using FSM.Blazor.Pages.MyAccount;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Radzen;

namespace FSM.Blazor.Shared
{
    public partial class MainLayout
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        bool sidebarExpanded = true;
        bool bodyExpanded = false;
        string userFullName = "";
        string loggedUserId;

        dynamic themes = new[]
        {
            new { Text = "Default Theme", Value = "default"},
            new { Text = "Dark Theme", Value="dark" },
            new { Text = "Software Theme", Value = "software"},
            new { Text = "Humanistic Theme", Value = "humanistic" },
            new { Text = "Standard Theme", Value = "standard" }
        };

        string Theme
        {
            get
            {
                return $".css";
            }
        }


        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/Login");
            }

            userFullName = user.Claims.Where(c => c.Type == CustomClaimTypes.FullName)
                               .Select(c => c.Value).SingleOrDefault();

            loggedUserId = user.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                               .Select(c => c.Value).SingleOrDefault();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                //    var example = ExampleService.FindCurrent(NavigationManager.ToAbsoluteUri(NavigationManager.Uri));

                //   await JSRuntime.InvokeVoidAsync("setTitle", ExampleService.TitleFor(example));
            }
        }

        void ChangeTheme(object value)
        {
            //ThemeState.CurrentTheme = value.ToString();
            NavigationManager.NavigateTo(NavigationManager.ToAbsoluteUri(NavigationManager.Uri).ToString());
        }

        async Task OpenChangePasswordDailog()
        {
            await DialogService.OpenAsync<ChangePassword>("Change Password",
                 new Dictionary<string, object>() { { "Id", loggedUserId } },  new DialogOptions() { Width = "500px", Height = "380px" });
        }

        async Task ManageMenuClickEvent(MenuItemEventArgs eventArgs)
        {
            if (eventArgs.Text == "Change Password")
            {
               await OpenChangePasswordDailog();
            }
        }
    }
}
