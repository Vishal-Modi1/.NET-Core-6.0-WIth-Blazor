using DataModels.VM.SubscriptionPlan;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace Web.UI.Data.SubscriptionPlan
{
    public class SubscriptionPlanService
    {
        private readonly HttpCaller _httpCaller;

        public SubscriptionPlanService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<SubscriptionPlanDataVM>> ListAsync(DependecyParams dependecyParams, SubscriptionDataTableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "subscriptionPlan/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<SubscriptionPlanDataVM>();
            }

            List<SubscriptionPlanDataVM> subscriptionPlans = JsonConvert.DeserializeObject<List<SubscriptionPlanDataVM>>(response.Data.ToString());

            return subscriptionPlans; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, SubscriptionPlanVM subscriptionPlanVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(subscriptionPlanVM);

            dependecyParams.URL = "subscriptionPlan/create";

            if (subscriptionPlanVM.Id > 0)
            {
                dependecyParams.URL = "subscriptionPlan/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"subscriptionPlan/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<SubscriptionPlanVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"subscriptionplan/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            SubscriptionPlanVM subscriptionPlanVM = new SubscriptionPlanVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                subscriptionPlanVM = JsonConvert.DeserializeObject<SubscriptionPlanVM>(response.Data.ToString());
            }

            return subscriptionPlanVM;
        }

        public async Task<CurrentResponse> UpdateStatus(DependecyParams dependecyParams, int id, bool isActive)
        {
            dependecyParams.URL = $"subscriptionPlan/updatestatus?id={id}&isActive={isActive}";
              
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> BuyPlan(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"subscriptionPlan/buyplan?id={id}";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
