using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.JSInterop;
using Web.UI.Utilities;

namespace Web.UI.Pages.Weather
{
    partial class Radar
    {
        string mapSrc = "";
        RadarMapConfigurationVM radarMapConfiguration = new();

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

            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            radarMapConfiguration = await RadarMapConfigurationService.GetDefault(dependecyParams);

            if (radarMapConfiguration.Id == 0)
            {
                SetDefaultValues();
            }
            else
            {
                await RefreshHeight();
                await RefreshWidth();
            }

            ChangeLoaderVisibilityAction(false);
        }

        private void SetDefaultValues()
        {
            radarMapConfiguration.Height = 700;
            radarMapConfiguration.Width = 1200;
        }

        public async Task OnWidthChange()
        {
            if (radarMapConfiguration.Width < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum width should be 500px");
                radarMapConfiguration.Width = 500;
                return;
            }

            await RefreshWidth();
        }

        public async Task OnHeightChange()
        {
            if (radarMapConfiguration.Height < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum height should be 500px");
                radarMapConfiguration.Height = 500;
                return;
            }

            await RefreshHeight();
        }

        private async Task RefreshHeight()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshRadarMapHeight", radarMapConfiguration.Height);
        }

        private async Task RefreshWidth()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshRadarMapWidth", radarMapConfiguration.Width);
        }

        public async Task Submit()
        {
            isBusyAddButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await RadarMapConfigurationService.SetDefault(dependecyParams, radarMapConfiguration);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }
    }
}
