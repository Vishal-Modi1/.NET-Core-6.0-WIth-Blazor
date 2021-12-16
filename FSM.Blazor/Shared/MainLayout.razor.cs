using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen.Blazor;
using System.Security.Claims;

namespace FSM.Blazor.Shared
{
    public partial class MainLayout
    {
        RadzenSidebar sidebar0;
        RadzenBody body0;
        bool sidebarExpanded = true;
        bool bodyExpanded = false;

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

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/");
            }
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
    }
}
