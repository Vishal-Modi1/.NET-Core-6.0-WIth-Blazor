using DataModels.VM.Common;
using DataModels.VM.User;
using FSM.Blazor.Data.User;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.User
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<UserDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        IList<UserDataVM> data;
        int count;
        bool isLoading;

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = DataGridFilterCreator.Create(args, "Name");

            data = await UserService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;            
        }

        async Task UseCreateDialog(UserDataVM userData)
        {
            await DialogService.OpenAsync<Create>($"Edit",
                  new Dictionary<string, object>() { { "userData", userData } },
                  new DialogOptions() { Width = "500px", Height = "380px" });

            await grid.Reload();

        }

        async Task DeleteAsync(int id)
        {
            CurrentResponse response = await UserService.DeleteAsync(_httpClient, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Use Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Use Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

    }
}
