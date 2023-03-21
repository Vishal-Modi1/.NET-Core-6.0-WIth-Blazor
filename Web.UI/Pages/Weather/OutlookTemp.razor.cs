using Microsoft.JSInterop;

namespace Web.UI.Pages.Weather
{
    public partial class OutlookTemp
    {
        string mapSrc = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://www.cpc.ncep.noaa.gov/products/predictions/610day/610temp.new.gif";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadOutlookTempMap", mapSrc);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
