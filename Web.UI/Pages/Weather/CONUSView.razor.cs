using DataModels.VM.Common;
using DataModels.VM.Weather;
using Microsoft.JSInterop;
using Web.UI.Utilities;

namespace Web.UI.Pages.Weather
{
    public partial class CONUSView
    {
        string mapSrc = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://www.wpc.ncep.noaa.gov/medr/nav_conus_pmsl.php";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadCONUSViewMap", mapSrc);

            ChangeLoaderVisibilityAction(false);
        } 
    }
}
