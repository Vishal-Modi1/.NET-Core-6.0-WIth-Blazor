using DataModels.VM.Common;
using DataModels.VM.BillingConfigurations;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Web.UI.Data.Company.Settings;
using Web.UI.Utilities;
using DataModels.Entities;
using DataModels.VM.Company;

namespace Web.UI.Pages.Company.DetailsView.Settings.BillingConfiguration
{
    partial class Create
    {
        [Parameter] public BillingConfigurationVM billingConfigurationData { get; set; }
        [Parameter] public CompanyVM CompanyData { get; set; }
        DependecyParams dependecyParams;

        protected override Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

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

        public async Task SetPropellerConfiguration()
        {
            isBusyAddButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.SetPropellerConfiguration(dependecyParams, CompanyData.Id, CompanyData.IsDisplayPropeller);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }
    }
}
