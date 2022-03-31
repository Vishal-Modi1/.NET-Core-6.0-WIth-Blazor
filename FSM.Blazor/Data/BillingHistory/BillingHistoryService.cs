using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.BillingHistory;
using Newtonsoft.Json;
using DataModels.VM.Common;

namespace FSM.Blazor.Data.BillingHistory
{
    public class BillingHistoryService
    {
        private readonly HttpCaller _httpCaller;

        public BillingHistoryService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<BillingHistoryDataVM>> ListAsync(IHttpClientFactory httpClient, DatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, "BillingHistory/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<BillingHistoryDataVM>();
            }

            List<BillingHistoryDataVM> billingHistoriesList = JsonConvert.DeserializeObject<List<BillingHistoryDataVM>>(response.Data.ToString());

            return billingHistoriesList;
        }

    }
}
