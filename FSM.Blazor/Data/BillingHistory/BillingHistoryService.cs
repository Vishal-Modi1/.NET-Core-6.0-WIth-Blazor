using DataModels.VM.BillingHistory;
using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.BillingHistory
{
    public class BillingHistoryService
    {
        private readonly HttpCaller _httpCaller;

        public BillingHistoryService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<BillingHistoryDataVM>> ListAsync(DependecyParams dependecyParams, BillingHistoryDatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "BillingHistory/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<BillingHistoryDataVM>();
            }

            List<BillingHistoryDataVM> billingHistoriesList = JsonConvert.DeserializeObject<List<BillingHistoryDataVM>>(response.Data.ToString());

            return billingHistoriesList;
        }

    }
}
