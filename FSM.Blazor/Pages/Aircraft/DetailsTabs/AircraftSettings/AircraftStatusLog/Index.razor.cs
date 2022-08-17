using DataModels.VM.AircraftEquipment;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Enums;
using DataModels.VM.Aircraft.AircraftStatusLog;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs.AircraftSettings.AircraftStatusLog
{
    partial class Index
    {
        [Parameter]
        public long AircraftId { get; set; }

        [Parameter]
        public long CreatedBy { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;
        string moduleName = "Aircraft", popupTitle;
        public bool isAllowToEdit;

        [CascadingParameter]
        public RadzenDataGrid<AircraftStatusLogDataVM> grid { get; set; }

        List<AircraftStatusLogDataVM> data;
        int count;
        bool isLoading, isBusyAddNewButton, isDisplayPopup;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        AircraftStatusLogVM aircraftStatusLogVM;
        OperationType operationType;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            bool isAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.Admin).Result;
            bool isSuperAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.SuperAdmin).Result;

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);

            bool isCreator = userId == CreatedBy;

            if (isAdmin || isSuperAdmin || isCreator)
            {
                isAllowToEdit = true;
            }

            base.OnInitialized();
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            AircraftEquipmentDatatableParams datatableParams = new AircraftEquipmentDatatableParams().Create(args, "Status");
            datatableParams.AircraftId = AircraftId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await AircraftStatusLogService.ListAsync(dependecyParams, datatableParams);

            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task DeleteAsync(long id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftStatusLogService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Status Log", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Status Log", response.Message);
                NotificationService.Notify(message);
            }
        }

        private void SetEditButtonState(long id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            //details.IsLoadingEditButton = isBusy;
        }

        async void AircraftEquipmentCreateDialog(long id, string title)
        {
            if (id == 0)
            {
                SetAddNewButtonState(true);
                operationType = OperationType.Create;
            }
            else
            {
                SetEditButtonState(id, true);
                operationType = OperationType.Edit;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
           // aircraftStatusLogVM = await AircraftStatusLogService.GetEquipmentDetailsAsync(dependecyParams, id);

            SetAddNewButtonState(false);

            aircraftStatusLogVM.AircraftId = AircraftId;

            isDisplayPopup = true;
            popupTitle = title;

            if (id == 0)
            {
                SetAddNewButtonState(false);
            }
            else
            {
                SetEditButtonState(id, false);
            }
        }

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
        }

        void OpenDeleteDialog(AircraftEquipmentDataVM aircraftEquipmentDataVM)
        {
            isDisplayPopup = true;
            popupTitle = "Delete Aircraft Status Log";
            operationType = OperationType.Delete;

            aircraftStatusLogVM = new AircraftStatusLogVM();
            aircraftStatusLogVM.Id = aircraftEquipmentDataVM.Id;
        }
    }
}
