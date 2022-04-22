using DataModels.VM.SubscriptionPlan;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace FSM.Blazor.Data.SubscriptionPlan
{
    public class SubscriptionPlanService
    {
        private readonly HttpCaller _httpCaller;

        public SubscriptionPlanService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<SubscriptionPlanDataVM>> ListAsync(IHttpClientFactory httpClient, SubscriptionDataTableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync( httpClient, "subscriptionPlan/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<SubscriptionPlanDataVM>();
            }

            List<SubscriptionPlanDataVM> subscriptionPlans = JsonConvert.DeserializeObject<List<SubscriptionPlanDataVM>>(response.Data.ToString());

            return subscriptionPlans; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, SubscriptionPlanVM subscriptionPlanVM)
        {
            string jsonData = JsonConvert.SerializeObject(subscriptionPlanVM);
            
            string url = "subscriptionPlan/create";

            if (subscriptionPlanVM.Id > 0)
            {
                url = "subscriptionPlan/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            string url = $"subscriptionPlan/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

        public async Task<SubscriptionPlanVM> GetDetailsAsync(IHttpClientFactory httpClient, long id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"subscriptionplan/getDetails?id={id}");

            SubscriptionPlanVM subscriptionPlanVM = new SubscriptionPlanVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                subscriptionPlanVM = JsonConvert.DeserializeObject<SubscriptionPlanVM>(response.Data.ToString());
            }

            return subscriptionPlanVM;
        }

        public async Task<CurrentResponse> UpdateStatus(IHttpClientFactory httpClient, int id, bool isActive)
        {
            string url = $"subscriptionPlan/updatestatus?id={id}&isActive={isActive}";
              
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);

            return response;
        }

        public async Task<CurrentResponse> BuyPlan(IHttpClientFactory httpClient, int id)
        {
            string url = $"subscriptionPlan/buyplan?id={id}";

            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);

            return response;
        }
    }
}
