using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;

namespace FSM.Blazor.Pages.SubscriptionPlan
{
    public partial class Create
    {
        [Parameter]
        public SubscriptionPlanVM subscriptionPlanData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isBusy = false;
        IEnumerable<string> multipleValues;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(subscriptionPlanData.ModuleIds))
            {
                string[] modules = subscriptionPlanData.ModuleIds.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                multipleValues = subscriptionPlanData.ModulesList.Where(p => modules.Contains(p.Id.ToString())).Select(p => p.Name).ToList();
            }
        }

        public async Task Submit(SubscriptionPlanVM subscriptionPlanData)
        {
            SetSaveButtonState(true);

            subscriptionPlanData.ModuleIds = String.Join(",",subscriptionPlanData.ModulesList.Where(p => multipleValues.Contains(p.Name)).Select(p => p.Id));

            CurrentResponse response = await SubscriptionPlanService.SaveandUpdateAsync(_httpClient, subscriptionPlanData);

            SetSaveButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Subscription Plan Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Subscription Plan Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
