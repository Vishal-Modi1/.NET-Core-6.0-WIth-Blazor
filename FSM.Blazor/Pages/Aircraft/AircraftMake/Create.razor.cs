using DataModels.VM.Common;
using DE = DataModels.Entities;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;


namespace FSM.Blazor.Pages.Aircraft.AircraftMake
{
    public partial class Create
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        DE.AircraftMake aircraftMake = new DE.AircraftMake();

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        public async Task Submit()
        {
            CurrentResponse response = await AircraftMakeService.SaveandUpdateAsync(_httpClient, aircraftMake);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Make", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Make", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
