using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Radzen;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.SubscriptionPlan
{
    partial class BuyNow
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        public string SubscriptionPlanId { get; set; }

        protected override Task OnInitializedAsync()
        {
            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("SubscriptionPlanId", out link);

            if (link.Count() == 0 || link[0] == "")
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
            SubscriptionPlanId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace("FlyManager", "");


            return base.OnInitializedAsync();
        }

        public async Task BuySubscriptionPlan()
        {
            CurrentResponse response = await SubscriptionPlanService.BuyPlan(_httpClient, Convert.ToInt32(SubscriptionPlanId));

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, response.Message, "");
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, response.Message, "");
                NotificationService.Notify(message);
            }
        }
    }
}
