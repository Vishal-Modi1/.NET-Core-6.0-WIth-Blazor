using DataModels.VM.BillingConfigurations;
using Microsoft.AspNetCore.Components;
using Web.UI.Data.Company.Settings;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView.Settings.BillingConfiguration
{
    partial class Index
    {
        [Parameter] public int CompanyIdParam { get; set; }
        BillingConfigurationVM billingConfiguration;

        protected override async Task OnInitializedAsync()
        {
            billingConfiguration = new BillingConfigurationVM();
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            billingConfiguration = await BillingConfigurationService.GetDefault(dependecyParams, CompanyIdParam);

            ChangeLoaderVisibilityAction(false);
            billingConfiguration.CompanyId = CompanyIdParam;
        }
    }
}
