using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FSM.Blazor.Pages.Administration
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        IEnumerable<MenuItem> menuItems;

        private bool isDisplayLoader { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            isDisplayLoader = true;

            menuItems = await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider);

            menuItems = menuItems.Where(p=>p.IsAdministrationModule == true).ToList();

            isDisplayLoader = false;
        }

        void Change(int value)
        {
            
            StateHasChanged();
        }
    }
}
