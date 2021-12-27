using DataModels.VM.Common;
using DataModels.VM.Company;
using FSM.Blazor.Data.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.Company
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<CompanyVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        IList<CompanyVM> data;
        int count;
        bool isLoading;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");

            data = await CompanyService.ListAsync(_httpClient,datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;            
        }

        async Task CompanyCreateDialog(CompanyVM companyData)
        {
            await DialogService.OpenAsync<Create>($"Edit",
                  new Dictionary<string, object>() { { "companyData", companyData } },
                  new DialogOptions() { Width = "500px", Height = "380px" });

            await grid.Reload();

        }

        async Task DeleteAsync(int id)
        {
            CurrentResponse response = await CompanyService.DeleteAsync(_httpClient, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Company Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Company Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

    }
}
