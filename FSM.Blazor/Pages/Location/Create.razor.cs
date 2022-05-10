using DataModels.VM.Common;
using DataModels.VM.Location;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using System.Collections.ObjectModel;
using FSM.Blazor.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Constants;
using Radzen.Blazor;

namespace FSM.Blazor.Pages.Location
{
    public partial class Create
    {
        [Parameter]
        public LocationVM locationData { get; set; }

        [Parameter]
        public Action GoToNext { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isLoading = false, isBusy = false,isAuthenticated = false;

        ReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
        int timezoneId;

        public RadzenTemplateForm<LocationVM> locationForm;

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            timezoneId = locationData.TimezoneId;

            isAuthenticated = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result);

            return base.OnInitializedAsync();
        }

        public async Task Submit(LocationVM locationData)
        {
            isLoading = true;
            SetSaveButtonState(true);

            locationData.TimezoneId = (short)timezoneId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LocationService.SaveandUpdateAsync(dependecyParams, locationData);

            SetSaveButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                dialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Location Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Location Details", response.Message);
                NotificationService.Notify(message);
            }

            isLoading = false;
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
