using Microsoft.JSInterop;

namespace Web.UI.Pages.Weather
{
    public partial class Forecast
    {
        string mapSrc = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://www.wpc.ncep.noaa.gov/noaa/noaad1.gif";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadForecastMap", mapSrc);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
