using DataModels.VM.AircraftEquipment;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using DataModels.VM.Common;
using Utilities;
using Web.UI.Utilities;
using DataModels.Constants;
using DataModels.Enums;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.Aircraft.DetailsTabs.AircraftEquipment
{
    partial class Index
    {
        [Parameter] public long AircraftId { get; set; }
        [Parameter] public long CreatedBy { get; set; }
        [CascadingParameter] public TelerikGrid<AircraftEquipmentDataVM> grid { get; set; }

        string moduleName = "Aircraft";
        public bool isAllowToEdit;

        List<AircraftEquipmentDataVM> data;
        string timeZone = "";
        AircraftEquipmentsVM aircraftEquipmentsVM;
        
        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            timeZone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
            bool isCreator = userId == CreatedBy;

            if (globalMembers.IsAdmin || globalMembers.IsSuperAdmin || isCreator)
            {
                isAllowToEdit = true;
            }

            base.OnInitialized();
        }

        async Task LoadData(GridReadEventArgs args)
        {
            AircraftEquipmentDatatableParams datatableParams = new DatatableParams().Create(args, "Status").Cast<AircraftEquipmentDatatableParams>();
            datatableParams.AircraftId = AircraftId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await AircraftEquipmentService.ListAsync(dependecyParams, datatableParams);

            data.ForEach(p =>
            {
                p.LogEntryDate = DateConverter.ToLocal(p.LogEntryDate.Value, timeZone);
                p.ManufacturerDate = DateConverter.ToLocal(p.ManufacturerDate.Value, timeZone);
            });

            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        async Task DeleteAsync(long id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftEquipmentService.DeleteAsync(dependecyParams, id);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        async void AircraftEquipmentCreateDialog(AircraftEquipmentDataVM aircraftEquipmentDataVM)
        {
            if (aircraftEquipmentDataVM.Id == 0)
            {
                isBusyAddButton = true;
                operationType = OperationType.Create;
                popupTitle = "Create Equipment";
            }
            else
            {
                aircraftEquipmentDataVM.IsLoadingEditButton = true;
                operationType = OperationType.Edit;
                popupTitle = "Update Equipment Details";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            aircraftEquipmentsVM = await AircraftEquipmentService.GetEquipmentDetailsAsync(dependecyParams, aircraftEquipmentDataVM.Id);

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

            aircraftEquipmentsVM.AircraftId = AircraftId;

            if (aircraftEquipmentDataVM.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                aircraftEquipmentDataVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
            base.StateHasChanged();
        }

        void CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
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
