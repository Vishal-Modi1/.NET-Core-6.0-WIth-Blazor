using DataModels.VM.Common;
using FSM.Blazor.Data.AircraftMake;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.AircraftMake;
using DataModels.Enums;
using DE = DataModels.Entities;
using Newtonsoft.Json;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.AircraftMake
{
    partial class Index
    {
        #region Params

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        [CascadingParameter] public RadzenDataGrid<AircraftMakeDataVM> grid { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<AircraftMakeDataVM> data;
        int count;
        bool isLoading, isBusyAddButton, isBusyEditButton;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = Module.Aircraft.ToString();

        bool isDisplayPopup { get; set; }
        string popupTitle { get; set; }

        OperationType operationType = OperationType.Create;
        DE.AircraftMake _aircraftMake { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            data = await AircraftMakeService.ListAsync(dependecyParams, datatableParams);

            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task AircraftMakeCreateDialog(AircraftMakeDataVM aircraftMake, string title)
        {
            _aircraftMake = new DE.AircraftMake();
            _aircraftMake.Id = aircraftMake.Id;
            _aircraftMake.Name = aircraftMake.Name;

            if (_aircraftMake.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
            }
            else
            {
                operationType = OperationType.Edit;
                SetEditButtonState(_aircraftMake.Id, true);
            }

            popupTitle = title;
            isDisplayPopup = true;

            if (_aircraftMake.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                SetEditButtonState(_aircraftMake.Id, false);
            }
        }

        private void SetEditButtonState(int id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            details.IsLoadingEditButton = isBusy;
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftMakeService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, "AircraftMake Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "AircraftMake Details", response.Message);
                NotificationService.Notify(message);
            }

            await CloseDialog(false);
        }

        async Task OpenDeleteDialog(AircraftMakeDataVM aircraftMake)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _aircraftMake = new DE.AircraftMake();

            _aircraftMake.Id = aircraftMake.Id;
            _aircraftMake.Name = aircraftMake.Name;

            popupTitle = "Delete Make";
        }
    }
}
