using DataModels.VM.Common;
using DataModels.VM.InstructorType;
using FSM.Blazor.Data.InstructorType;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.InstructorType
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<InstructorTypeVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        IList<InstructorTypeVM> data;
        int count;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        bool isLoading;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string searchText;

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");
            pageSize = datatableParams.Length;
            datatableParams.SearchText = searchText;

            data = await InstructorTypeService.ListAsync(_httpClient,datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;            
        }

        async Task InstructorTypeCreateDialog(InstructorTypeVM instructorTypeData)
        {
            await DialogService.OpenAsync<Create>($"Edit",
                  new Dictionary<string, object>() { { "instructorTypeData", instructorTypeData } },
                  new DialogOptions() { Width = "500px", Height = "380px" });

            await grid.Reload();

        }

        async Task DeleteAsync(int id)
        {
            CurrentResponse response = await InstructorTypeService.DeleteAsync(_httpClient, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "InstructorType Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "InstructorType Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

    }
}
