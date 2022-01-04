using DataModels.VM.Common;
using DE = DataModels.Entities;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;


namespace FSM.Blazor.Pages.Aircraft.AircraftModel
{
    public partial class Create
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        DE.AircraftModel aircraftModel = new DE.AircraftModel();

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        public async Task Submit()
        {
            CurrentResponse response = await AircraftModelService.SaveandUpdateAsync(_httpClient, aircraftModel);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Model", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Model", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
