using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;


namespace FSM.Blazor.Pages.Company
{
    public partial class Create
    {
        [Parameter]
        public CompanyVM companyData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        public async Task Submit(CompanyVM companyData)
        {
            CurrentResponse response = await CompanyService.SaveandUpdateAsync(_httpClient, companyData);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                dialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Company Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Company Details", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
