using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;
using Microsoft.AspNetCore.Components;
using Utilities;
using Web.UI.Utilities;
using DataModels.Constants;

namespace Web.UI.Pages.Aircraft.DetailsTabs.AircraftEquipment
{
    partial class Create
    {
        [Parameter] public AircraftEquipmentsVM aircraftEquipmentsVM { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public int aircraftTTInstall, partTTInstall = 0;

        protected override Task OnInitializedAsync()
        {
            aircraftTTInstall = aircraftEquipmentsVM.AircraftTTInstall.GetValueOrDefault();
            partTTInstall = aircraftEquipmentsVM.PartTTInstall.GetValueOrDefault();
            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            aircraftEquipmentsVM.AircraftTTInstall = aircraftTTInstall;
            aircraftEquipmentsVM.PartTTInstall = partTTInstall;

            string timeZone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);

            aircraftEquipmentsVM.LogEntryDate = DateConverter.ToUTC(aircraftEquipmentsVM.LogEntryDate.Value, timeZone);
            aircraftEquipmentsVM.ManufacturerDate = DateConverter.ToUTC(aircraftEquipmentsVM.ManufacturerDate.Value, timeZone);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftEquipmentService.SaveandUpdateAsync(dependecyParams, aircraftEquipmentsVM);

            isBusySubmitButton = false;

            ManageResponse(response, "Aircraft Equipment Details", true);
        }

        private void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
