using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using DataModels.VM.Document;
using FSM.Blazor.Utilities;

namespace FSM.Blazor.Pages.Document.DocumentTag
{
    public partial class Create
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        DocumentTagVM documentTagVM = new DocumentTagVM();

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        public async Task Submit()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentService.SaveTagAsync(dependecyParams, documentTagVM);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Document Tag", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Document Tag", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
