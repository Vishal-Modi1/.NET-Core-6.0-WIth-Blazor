using DataModels.VM.Common;
using DataModels.VM.BillingConfigurations;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Web.UI.Data.Company.Settings;
using Web.UI.Utilities;
using DataModels.Entities;

namespace Web.UI.Pages.Company.DetailsView.Settings.BillingConfiguration
{
    partial class Create
    {
        [Parameter] public BillingConfigurationVM billingConfigurationData { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            DataModels.Entities.BillingConfiguration billingConfigurationInfo = new();
            billingConfigurationInfo.Id = billingConfigurationData.Id;
            billingConfigurationInfo.CompanyId = billingConfigurationData.CompanyId;
            billingConfigurationInfo.BillingFollows = billingConfigurationData.BillingFollows;

            CurrentResponse response = await BillingConfigurationService.SetDefault(dependecyParams, billingConfigurationInfo);
            
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                billingConfigurationData = JsonConvert.DeserializeObject<BillingConfigurationVM>(response.Data.ToString());
            }

            isBusySubmitButton = false;
        }
    }
}
