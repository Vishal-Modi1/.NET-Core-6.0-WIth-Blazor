using DataModels.VM.Common;
using DataModels.VM.Weather;
using Microsoft.JSInterop;
using Web.UI.Utilities;

namespace Web.UI.Pages.Weather
{
    public partial class NOAARadar
    {
        string mapSrc = "";
        NOAARadarMapConfigurationVM noaaRadarMapConfiguration = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://radar.weather.gov/";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadNOAARadarMap", mapSrc);

            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            noaaRadarMapConfiguration = await NOAARadarMapConfigurationService.GetDefault(dependecyParams);

            if (noaaRadarMapConfiguration.Id == 0)
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
            noaaRadarMapConfiguration.Height = 700;
            noaaRadarMapConfiguration.Width = 1200;
        }

        public async Task OnWidthChange()
        {
            if (noaaRadarMapConfiguration.Width < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum width should be 500px");
                noaaRadarMapConfiguration.Width = 500;
                return;
            }

            await RefreshWidth();
        }

        public async Task OnHeightChange()
        {
            if (noaaRadarMapConfiguration.Height < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum height should be 500px");
                noaaRadarMapConfiguration.Height = 500;
                return;
            }

            await RefreshHeight();
        }

        private async Task RefreshHeight()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshRadarMapHeight", noaaRadarMapConfiguration.Height);
        }

        private async Task RefreshWidth()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshRadarMapWidth", noaaRadarMapConfiguration.Width);
        }

        public async Task Submit()
        {
            isBusyAddButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await NOAARadarMapConfigurationService.SetDefault(dependecyParams, noaaRadarMapConfiguration);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }
    }
}
