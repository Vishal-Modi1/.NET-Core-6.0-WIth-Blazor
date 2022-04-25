using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using System.Collections.ObjectModel;

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
        bool isLoading = false, isBusy = false;

        ReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
        int? primaryServiceId;

        protected override Task OnInitializedAsync()
        {
            primaryServiceId = companyData.PrimaryServiceId;
            return base.OnInitializedAsync();
        }

        public async Task Submit(CompanyVM companyData)
        {
            isLoading = true;
            SetSaveButtonState(true);

            companyData.PrimaryServiceId = primaryServiceId == null ? null : (short)primaryServiceId;
            CurrentResponse response = await CompanyService.SaveandUpdateAsync(_httpClient, companyData);

            SetSaveButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
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

            isLoading = false;
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
