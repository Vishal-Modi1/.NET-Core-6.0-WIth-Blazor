using Microsoft.JSInterop;

namespace Web.UI.Pages.Weather
{
    public partial class OutlookPrecipitation
    {
        string mapSrc = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://www.cpc.ncep.noaa.gov/products/predictions/610day/610prcp.new.gif";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadOutlookPrecipitationMap", mapSrc);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
