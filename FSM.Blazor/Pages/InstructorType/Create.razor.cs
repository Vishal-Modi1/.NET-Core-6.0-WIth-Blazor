using DataModels.VM.Common;
using DataModels.VM.InstructorType;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;


namespace FSM.Blazor.Pages.InstructorType
{
    public partial class Create
    {
        [Parameter]
        public InstructorTypeVM instructorTypeData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        public async Task Submit(InstructorTypeVM instructorTypeData)
        {
            CurrentResponse response = await InstructorTypeService.SaveandUpdateAsync(_httpClient, instructorTypeData);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                dialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "InstructorType Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "InstructorType Details", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
