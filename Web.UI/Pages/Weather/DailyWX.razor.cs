using DataModels.VM.Weather;
using Microsoft.JSInterop;

namespace Web.UI.Pages.Weather
{
    public partial class DailyWX
    {
        string mapSrc = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://www.wpc.ncep.noaa.gov/dailywxmap/";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadDailyWXMap", mapSrc);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
