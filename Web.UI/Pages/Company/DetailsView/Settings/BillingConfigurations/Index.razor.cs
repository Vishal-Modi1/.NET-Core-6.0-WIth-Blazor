using DataModels.VM.BillingConfigurations;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using Web.UI.Data.Company.Settings;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView.Settings.BillingConfiguration
{
    partial class Index
    {
        [Parameter] public CompanyVM CompanyData { get; set; }
        BillingConfigurationVM billingConfiguration;

        protected override async Task OnInitializedAsync()
        {
            billingConfiguration = new BillingConfigurationVM();
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            billingConfiguration = await BillingConfigurationService.GetDefault(dependecyParams, CompanyData.Id);

            ChangeLoaderVisibilityAction(false);
            billingConfiguration.CompanyId = CompanyData.Id;
        }
    }
}
