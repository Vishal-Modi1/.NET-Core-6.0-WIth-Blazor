using DataModels.VM.Common;
using DataModels.VM.Weather;
using Microsoft.JSInterop;
using Web.UI.Utilities;

namespace Web.UI.Pages.Maps
{
    partial class AircraftLiveTracker
    {
        string mapSrc = "";
        AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfiguration = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://globe.adsbexchange.com/";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadAircraftLiveTrackerMap", mapSrc);

            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            aircraftLiveTrackerMapConfiguration = await AircraftLiveTrackerMapConfigurationService.GetDefault(dependecyParams);

            if (aircraftLiveTrackerMapConfiguration.Id == 0)
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
            aircraftLiveTrackerMapConfiguration.Height = 700;
            aircraftLiveTrackerMapConfiguration.Width = 1200;
        }

        public async Task OnWidthChange()
        {
            if (aircraftLiveTrackerMapConfiguration.Width < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum width should be 500px");
                aircraftLiveTrackerMapConfiguration.Width = 500;
                return;
            }

            await RefreshWidth();
        }

        public async Task OnHeightChange()
        {
            if (aircraftLiveTrackerMapConfiguration.Height < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum height should be 500px");
                aircraftLiveTrackerMapConfiguration.Height = 500;
                return;
            }

            await RefreshHeight();
        }

        private async Task RefreshHeight()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshAircraftLiveTrackerMapHeight", aircraftLiveTrackerMapConfiguration.Height);
        }

        private async Task RefreshWidth()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshAircraftLiveTrackerMapWidth", aircraftLiveTrackerMapConfiguration.Width);
        }

        public async Task Submit()
        {
            isBusyAddButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftLiveTrackerMapConfigurationService.SetDefault(dependecyParams, aircraftLiveTrackerMapConfiguration);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }
    }
}
