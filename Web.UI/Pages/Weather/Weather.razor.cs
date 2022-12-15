using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.JSInterop;
using Web.UI.Utilities;

namespace Web.UI.Pages.Weather
{
    partial class Weather
    {
        WindyMapConfiguration windyMapConfiguration = new();
        List<DropDownStringValues> windSpeedList = new();
        List<DropDownStringValues> temperaturesList = new();
        List<DropDownStringValues> forecastList = new();
        string mapSrc = "";
        bool isFilterBarVisible;

        protected override async Task OnInitializedAsync()
        {
            SetWindDropDownList();
            SetTemperatureDropDownList();
            SetForecastWindDropDownList();

            await LoadData();
        }

        private async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            windyMapConfiguration = await WindyMapConfigurationService.GetDefault(dependecyParams);

            if (windyMapConfiguration.Id == 0)
            {
                SetDefaultValues();
            }
            else
            {
                await RefreshMap();
                await RefreshHeight();
                await RefreshWidth();
            }

            ChangeLoaderVisibilityAction(false);
        }

        private void SetDefaultValues()
        {
            windyMapConfiguration.Wind = "default";
            windyMapConfiguration.Temperature = "default";
            windyMapConfiguration.Forecast = "now";
            windyMapConfiguration.Height = 450;
            windyMapConfiguration.Width = 650;
        }

        public async Task OnWindSpeedChange(string value)
        {
            windyMapConfiguration.Wind = value;
            await RefreshMap();
        }

        public async Task OnTemperatureTypeChange(string value)
        {
            windyMapConfiguration.Temperature = value;
            await RefreshMap();
        }

        public async Task OnForecastChange(string value)
        {
            windyMapConfiguration.Forecast = value;
            await RefreshMap();
        }

        public async Task OnWidthChange()
        {
            await RefreshWidth();
        }

        public async Task OnHeightChange()
        {
            await RefreshHeight();
        }

        private async Task RefreshMap()
        {
            mapSrc = $"https://embed.windy.com/embed2.html?lat=21.189&lon=72.829&detailLat=21.189&detailLon=72.829&width={windyMapConfiguration.Width}&height={windyMapConfiguration.Height}&zoom=5&level=surface&overlay=wind&product=ecmwf&menu=&message=&marker=&calendar={windyMapConfiguration.Forecast}&pressure=&type=map&location=coordinates&detail=&metricWind={windyMapConfiguration.Wind}&metricTemp={windyMapConfiguration.Temperature}&radarRange=-1";
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshWindyMap", mapSrc);
        }

        private async Task RefreshHeight()
        {
            mapSrc = $"https://embed.windy.com/embed2.html?lat=21.189&lon=72.829&detailLat=21.189&detailLon=72.829&width={windyMapConfiguration.Width}&height={windyMapConfiguration.Height}&zoom=5&level=surface&overlay=wind&product=ecmwf&menu=&message=&marker=&calendar={windyMapConfiguration.Forecast}&pressure=&type=map&location=coordinates&detail=&metricWind={windyMapConfiguration.Wind}&metricTemp={windyMapConfiguration.Temperature}&radarRange=-1";
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshHeight", windyMapConfiguration.Height);
        }

        private async Task RefreshWidth()
        {
            mapSrc = $"https://embed.windy.com/embed2.html?lat=21.189&lon=72.829&detailLat=21.189&detailLon=72.829&width={windyMapConfiguration.Width}&height={windyMapConfiguration.Height}&zoom=5&level=surface&overlay=wind&product=ecmwf&menu=&message=&marker=&calendar={windyMapConfiguration.Forecast}&pressure=&type=map&location=coordinates&detail=&metricWind={windyMapConfiguration.Wind}&metricTemp={windyMapConfiguration.Temperature}&radarRange=-1";
            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshWidth", windyMapConfiguration.Width);
        }

        public async Task Submit()
        {
            isBusyAddButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await WindyMapConfigurationService.SetDefault(dependecyParams, windyMapConfiguration);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }

        #region dropdown value

        private void SetWindDropDownList()
        {
            windSpeedList.Add(new DropDownStringValues() { Id = "default", Name = "Default units" });
            windSpeedList.Add(new DropDownStringValues() { Id = "kt", Name = "kt" });
            windSpeedList.Add(new DropDownStringValues() { Id = "m/s", Name = "m/s" });
            windSpeedList.Add(new DropDownStringValues() { Id = "km/h", Name = "km/h" });
            windSpeedList.Add(new DropDownStringValues() { Id = "mph", Name = "mph" });
            windSpeedList.Add(new DropDownStringValues() { Id = "bft", Name = "bft" });
        }

        private void SetTemperatureDropDownList()
        {
            temperaturesList.Add(new DropDownStringValues() { Id = "default", Name = "Default units" });
            temperaturesList.Add(new DropDownStringValues() { Id = "°C", Name = "°C" });
            temperaturesList.Add(new DropDownStringValues() { Id = "°F", Name = "°F" });
        }

        private void SetForecastWindDropDownList()
        {
            forecastList.Add(new DropDownStringValues() { Id = "now", Name = "Now" });
            forecastList.Add(new DropDownStringValues() { Id = "12", Name = "Next 12 hours" });
            forecastList.Add(new DropDownStringValues() { Id = "24", Name = "Next 24 hours" });
        }

        #endregion
    }
}
