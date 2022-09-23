using DataModels.VM.Common;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.Administration
{
    partial class Index
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ChangeLoaderVisibilityAction(true);
             
                menuItems = await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider);
                menuItems = menuItems.Where(p => p.IsAdministrationModule == true).ToList();

                ChangeLoaderVisibilityAction(false);
                
                base.StateHasChanged();
            }
        }

    }
}
