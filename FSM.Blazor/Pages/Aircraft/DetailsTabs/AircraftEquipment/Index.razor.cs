using DataModels.VM.AircraftEquipment;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using DataModels.VM.Common;
using Utilities;
using FSM.Blazor.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Enums;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs.AircraftEquipment
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
        string moduleName = "Aircraft";
        public bool isAllowToEdit;

        [CascadingParameter]
        public RadzenDataGrid<AircraftEquipmentDataVM> grid { get; set; }

        List<AircraftEquipmentDataVM> data;
        int count;
        bool isLoading, isBusyAddNewButton, isDisplayPopup;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        string timeZone = "", popupTitle;
        AircraftEquipmentsVM aircraftEquipmentsVM;
        OperationType operationType;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            timeZone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);

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
            data = await AircraftEquipmentService.ListAsync(dependecyParams, datatableParams);

            data.ForEach(p =>
            {
                p.LogEntryDate = DateConverter.ToLocal(p.LogEntryDate.Value, timeZone);
                p.ManufacturerDate = DateConverter.ToLocal(p.ManufacturerDate.Value, timeZone);
            });

            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task DeleteAsync(long id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftEquipmentService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Equipment Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Equipment Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        private void SetEditButtonState(long id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            details.IsLoadingEditButton = isBusy;
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
            aircraftEquipmentsVM = await AircraftEquipmentService.GetEquipmentDetailsAsync(dependecyParams, id);

            if (aircraftEquipmentsVM.LogEntryDate == null)
            {
                aircraftEquipmentsVM.LogEntryDate = DateConverter.ToLocal(DateTime.UtcNow, timeZone);
            }
            else
            {
                aircraftEquipmentsVM.LogEntryDate = DateConverter.ToLocal(aircraftEquipmentsVM.LogEntryDate.Value, timeZone);
            }

            if (aircraftEquipmentsVM.ManufacturerDate == null)
            {
                aircraftEquipmentsVM.ManufacturerDate = DateConverter.ToLocal(DateTime.UtcNow, timeZone);
            }
            else
            {
                aircraftEquipmentsVM.ManufacturerDate = DateConverter.ToLocal(aircraftEquipmentsVM.ManufacturerDate.Value, timeZone);
            }

            SetAddNewButtonState(false);

            aircraftEquipmentsVM.AirCraftId = AircraftId;

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
            popupTitle = "Delete Equipment Item";
            operationType = OperationType.Delete;

            aircraftEquipmentsVM = new AircraftEquipmentsVM();
            aircraftEquipmentsVM.Item = aircraftEquipmentDataVM.Item;
            aircraftEquipmentsVM.Id = aircraftEquipmentDataVM.Id;
        }
    }
}
