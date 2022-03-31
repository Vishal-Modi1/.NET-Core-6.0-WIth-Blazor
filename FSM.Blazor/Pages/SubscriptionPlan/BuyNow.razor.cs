using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace FSM.Blazor.Pages.SubscriptionPlan
{
    partial class BuyNow
    {
        public string SubscriptionPlanId { get; set; }

        protected override Task OnInitializedAsync()
        {
            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("AircraftId", out link);

            if (link.Count() == 0 || link[0] == "")
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
            SubscriptionPlanId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace("FlyManager", "");


            return base.OnInitializedAsync();
        }
    }
}
