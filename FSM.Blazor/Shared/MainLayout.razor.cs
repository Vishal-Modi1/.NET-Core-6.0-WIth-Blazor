using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;

namespace FSM.Blazor.Shared
{
    public partial class MainLayout
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

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


        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo($"/Login");
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
