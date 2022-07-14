using DataModels.VM.Common;
using DataModels.VM.Location;
using FSM.Blazor.Data.Location;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Enums;
using Newtonsoft.Json;

namespace FSM.Blazor.Pages.Location
{
    partial class Index
    {
        #region Params

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [CascadingParameter] public RadzenDataGrid<LocationDataVM> grid { get; set; }


        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<LocationDataVM> data;
        int count;
        bool isLoading, isDisplayPopup, isBusyAddButton;
        string searchText, moduleName = Module.Location.ToString(), message, popupTitle;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        OperationType operationType = OperationType.Create;
        LocationVM locationData;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            //if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            //{
            //    NavigationManager.NavigateTo("/Dashboard");
            //}
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams().Create(args, "AirportCode");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await LocationService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task LocationCreateDialog(int id, string title)
        {
            if (id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
            }
            else
            {
                operationType = OperationType.Edit;
                SetEditButtonState(id, true);
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            locationData = new LocationVM();

            CurrentResponse response = await LocationService.GetDetailsAsync(dependecyParams, id);

            if(response != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                locationData = JsonConvert.DeserializeObject<LocationVM>(response.Data.ToString());
                locationData.Timezones = await TimezoneService.ListDropDownValues(dependecyParams);
            }

            if (id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                SetEditButtonState(id, false);
            }

            isDisplayPopup = true;
            popupTitle = title;
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LocationService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                await CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Location Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Location Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
        }

        async Task OpenDeleteDialog(LocationDataVM locationDataVM)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Location";

            locationData = new LocationVM();

            locationData.Id = locationDataVM.Id;
            locationData.AirportCode = locationDataVM.AirportCode;
        }

        private void SetEditButtonState(int id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            details.IsLoadingEditButton = isBusy;
        }
    }
}
