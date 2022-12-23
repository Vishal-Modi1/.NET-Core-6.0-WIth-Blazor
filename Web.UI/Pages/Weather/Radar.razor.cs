using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.JSInterop;
using Web.UI.Utilities;

namespace Web.UI.Pages.Weather
{
    partial class Radar
    {
        string mapSrc = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);
            
            mapSrc = $"https://weather.cod.edu/satrad/?parms=continental-conus-13-24-0-100-1&checked=map&colorbar=data";
            
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadRadarMap", mapSrc);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
