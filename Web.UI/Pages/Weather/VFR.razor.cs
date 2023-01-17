using DataModels.VM.Weather;
using DataModels.VM.Common;
using Microsoft.JSInterop;
using Web.UI.Utilities;
using DataModels.Entities;

namespace Web.UI.Pages.Weather
{
    partial class VFR
    {
        string mapSrc = "";
        VFRMapConfigurationVM vFRMapConfiguration = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadMap();
        }

        private async Task LoadMap()
        {
            ChangeLoaderVisibilityAction(true);

            mapSrc = $"https://vfrmap.com/";

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("LoadVFRMap", mapSrc);

            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            vFRMapConfiguration = await VFRMapConfigurationService.GetDefault(dependecyParams);

            if (vFRMapConfiguration.Id == 0)
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
            vFRMapConfiguration.Height = 700;
            vFRMapConfiguration.Width = 1200;
        }

        public async Task OnWidthChange()
        {
            if (vFRMapConfiguration.Width < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum width should be 500px");
                vFRMapConfiguration.Width = 500;
                return;
            }

            await RefreshWidth();
        }

        public async Task OnHeightChange()
        {
            if (vFRMapConfiguration.Height < 500)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Minimum height should be 500px");
                vFRMapConfiguration.Height = 500;
                return;
            }

            await RefreshHeight();
        }

        private async Task RefreshHeight()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshVFRMapHeight", vFRMapConfiguration.Height);
        }

        private async Task RefreshWidth()
        {
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshVFRMapWidth", vFRMapConfiguration.Width);
        }

        public async Task Submit()
        {
            isBusyAddButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await VFRMapConfigurationService.SetDefault(dependecyParams, vFRMapConfiguration);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }
    }
}
