using DataModels.VM.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;
using DataModels.Entities;
using DataModels.VM.BillingConfigurations;

namespace Web.UI.Data.Company.Settings
{
    public class BillingConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public BillingConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, BillingConfiguration billingConfiguration)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(billingConfiguration);
            dependecyParams.URL = $"billingConfiguration/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<BillingConfigurationVM> GetDefault(DependecyParams dependecyParams, int companyId)
        {
            try
            {
                dependecyParams.URL = $"billingConfiguration/GetDefault?companyId={companyId}";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                BillingConfigurationVM billingConfiguration = JsonConvert.DeserializeObject<BillingConfigurationVM>(response.Data.ToString());

                return billingConfiguration;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
