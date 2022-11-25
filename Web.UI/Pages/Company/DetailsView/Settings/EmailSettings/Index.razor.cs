using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Utilities;
using Web.UI.Data.EmailConfiguration;
using Web.UI.Extensions;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView.Settings.Notifications
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<EmailConfigurationDataVM> grid { get; set; }
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

        async Task LoadData(GridReadEventArgs args)
        {
            datatableParams = new DatatableParams().Create(args, "Email");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            var data = await EmailConfigurationService.ListAsync(dependecyParams, datatableParams);

            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        void CloseDialog(bool reloadGrid)
        {
            if (reloadGrid)
            {
                grid.Rebind();
            }

            isDisplayPopup = false;
        }

        async Task OpenCreateDialog(EmailConfigurationDataVM emailConfigurationDataVM)
        {
            if (emailConfigurationDataVM.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Create New EmailConfiguration";
            }
            else
            {
                operationType = OperationType.Edit;
                emailConfigurationDataVM.IsLoadingEditButton = true;
                popupTitle = "Update EmailConfiguration";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            emailConfiguration = await EmailConfigurationService.GetDetailsAsync(dependecyParams, emailConfigurationDataVM.Id);

            if (emailConfigurationDataVM.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                emailConfigurationDataVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
            base.StateHasChanged();
        }
    }
}
