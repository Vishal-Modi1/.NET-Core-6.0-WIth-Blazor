using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Pages.Administration
{
    partial class Index
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        
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
