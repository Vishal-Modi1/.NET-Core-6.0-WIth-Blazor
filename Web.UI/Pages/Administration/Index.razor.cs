using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Pages.Administration
{
    partial class Index
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        List<MenuItem> menuItems = new List<MenuItem>();

        private bool isDisplayLoader { get; set; } = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                isDisplayLoader = true;
             
                menuItems = await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider);
                menuItems = menuItems.Where(p => p.IsAdministrationModule == true).ToList();

                isDisplayLoader = false;

                base.StateHasChanged();
            }
        }

    }
}
