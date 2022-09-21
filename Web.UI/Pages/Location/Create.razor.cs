using DataModels.VM.Common;
using DataModels.VM.Location;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Web.UI.Pages.Location
{
    public partial class Create
    {
        [Parameter] public LocationVM locationData { get; set; }
        [Parameter] public Action GoToNext { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        int timezoneId;

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            timezoneId = locationData.TimezoneId;

         //   isAuthenticated = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result);

            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = false;
            locationData.TimezoneId = (short)timezoneId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LocationService.SaveandUpdateAsync(dependecyParams, locationData);

            isBusySubmitButton = false;

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
