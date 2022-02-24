using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using Utilities;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Constants;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs.AircraftEquipment
{
    partial class Create
    {
        [Parameter]
        public AircraftEquipmentsVM AirCraftEquipmentsVM { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isBusySaveButton;

        public async Task Submit(AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            SetSaveButtonState(true);

            string timeZone = ClaimManager.GetClaimValue(authenticationStateProvider, CustomClaimTypes.TimeZone);

            airCraftEquipmentsVM.LogEntryDate = DateConverter.ToUTC(airCraftEquipmentsVM.LogEntryDate.Value, timeZone);
            airCraftEquipmentsVM.ManufacturerDate = DateConverter.ToUTC(airCraftEquipmentsVM.ManufacturerDate.Value, timeZone);

            CurrentResponse response = await AircraftEquipmentService.SaveandUpdateAsync(_httpClient, airCraftEquipmentsVM);

            SetSaveButtonState(false);

            ManageResponse(response, "Aircraft Equipment Details", true);
        }

        private void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (isCloseDialog)
                {
                    DialogService.Close(true);
                }

                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }
        }

        private void SetSaveButtonState(bool isBusy)
        {
            isBusySaveButton = isBusy;
            StateHasChanged();
        }
    }
}
