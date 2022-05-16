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
using Microsoft.Extensions.Caching.Memory;

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
        bool isLoading, isBusyAddNewButton;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;

        string timeZone = "";

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
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Equipment Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Equipment Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async void AircraftEquipmentCreateDialog(long id, string title)
        {
            SetAddNewButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            AircraftEquipmentsVM airCraftEquipmentsVM = await AircraftEquipmentService.GetEquipmentDetailsAsync(dependecyParams, id);

            if (airCraftEquipmentsVM.LogEntryDate == null)
            {
                airCraftEquipmentsVM.LogEntryDate = DateConverter.ToLocal(DateTime.UtcNow, timeZone);
            }
            else
            {
                airCraftEquipmentsVM.LogEntryDate = DateConverter.ToLocal(airCraftEquipmentsVM.LogEntryDate.Value, timeZone);
            }

            if (airCraftEquipmentsVM.ManufacturerDate == null)
            {
                airCraftEquipmentsVM.ManufacturerDate = DateConverter.ToLocal(DateTime.UtcNow, timeZone);
            }
            else
            {
                airCraftEquipmentsVM.ManufacturerDate = DateConverter.ToLocal(airCraftEquipmentsVM.ManufacturerDate.Value, timeZone);
            }

            SetAddNewButtonState(false);

            airCraftEquipmentsVM.AirCraftId = AircraftId;

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "airCraftEquipmentsVM", airCraftEquipmentsVM } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            await grid.Reload();
        }

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }
    }
}
