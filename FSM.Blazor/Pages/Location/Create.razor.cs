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
        [Parameter] public LocationVM locationData { get; set; }

        [Parameter] public Action GoToNext { get; set; }

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isBusy = false ;

        int timezoneId;

        public RadzenTemplateForm<LocationVM> locationForm;

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            timezoneId = locationData.TimezoneId;

         //   isAuthenticated = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result);

            return base.OnInitializedAsync();
        }

        public async Task Submit(LocationVM locationData)
        {
            SetSaveButtonState(true);

            locationData.TimezoneId = (short)timezoneId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Location Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Location Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
