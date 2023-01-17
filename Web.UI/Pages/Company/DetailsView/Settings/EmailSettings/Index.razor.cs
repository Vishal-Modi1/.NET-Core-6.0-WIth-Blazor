using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView.Settings.Notifications
{
    partial class Index
    {
        [Parameter] public int CompanyIdParam { get; set; }

        EmailConfigurationVM emailConfiguration;
        DatatableParams datatableParams;

        protected override async Task OnInitializedAsync()
        {
            emailConfiguration = new EmailConfigurationVM();
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            emailConfiguration = await EmailConfigurationService.GetDetailsAsync(dependecyParams, 0);

            ChangeLoaderVisibilityAction(false);
            emailConfiguration.CompanyId = CompanyIdParam;
        }

        void CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;
        }
    }
}
