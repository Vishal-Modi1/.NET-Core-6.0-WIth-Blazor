using DataModels.VM.Common;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.Administration
{
    partial class Index
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        TelerikTabStrip telerikTabStrip;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 ChangeLoaderVisibilityAction(true);
             
                menuItems = await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider);
                menuItems = menuItems.Where(p => p.IsAdministrationModule == true).ToList();

                 ChangeLoaderVisibilityAction(false);
                telerikTabStrip.ActiveTabIndex = 4;
                
                base.StateHasChanged();
            }
        }

    }
}
