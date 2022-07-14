using DataModels.VM.Common;
using FSM.Blazor.Data.AircraftModel;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.AircraftModel;
using DataModels.Enums;
using DE = DataModels.Entities;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.AircraftModel
{
    partial class Index
    {
        #region Params

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        [CascadingParameter] public RadzenDataGrid<AircraftModelDataVM> grid { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<AircraftModelDataVM> data;
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
        DE.AircraftModel _aircraftModel { get; set; }

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

            data = await AircraftModelService.ListAsync(dependecyParams, datatableParams);

            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task AircraftModelCreateDialog(AircraftModelDataVM aircraftModel, string title)
        {
            _aircraftModel = new DE.AircraftModel();
            _aircraftModel.Id = aircraftModel.Id;
            _aircraftModel.Name = aircraftModel.Name;

            if (_aircraftModel.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
            }
            else
            {
                operationType = OperationType.Edit;
                SetEditButtonState(_aircraftModel.Id, true);
            }

            popupTitle = title;
            isDisplayPopup = true;

            if (_aircraftModel.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                SetEditButtonState(_aircraftModel.Id, false);
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
            CurrentResponse response = await AircraftModelService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Model Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Model Details", response.Message);
                NotificationService.Notify(message);
            }

            await CloseDialog(false);
        }

        async Task OpenDeleteDialog(AircraftModelDataVM aircraftModel)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _aircraftModel = new DE.AircraftModel();

            _aircraftModel.Id = aircraftModel.Id;
            _aircraftModel.Name = aircraftModel.Name;

            popupTitle = "Delete Model";
        }
    }
}
