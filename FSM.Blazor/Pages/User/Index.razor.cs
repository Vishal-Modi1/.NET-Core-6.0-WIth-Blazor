using DataModels.VM.Common;
using DataModels.VM.User;
using FSM.Blazor.Data.User;
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

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> roleFilter { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        IList<UserDataVM> data;
        UserFilterVM userFilterVM;
        IList<DropDownValues> CompanyFilterDropdown;
        IList<DropDownValues> RoleFilterDropdown;
        int CompanyId; int RoleId;
        int count;
        bool isLoading;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;

        protected override async Task OnInitializedAsync()
        {
            userFilterVM = await UserService.GetFiltersAsync(_httpClient);
            CompanyFilterDropdown = userFilterVM.Companies;
            RoleFilterDropdown = userFilterVM.UserRoles;
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            UserDatatableParams datatableParams = new UserDatatableParams().Create(args, "Name");

            datatableParams.CompanyId = CompanyId;
            datatableParams.SearchText = searchText;
            datatableParams.RoleId = RoleId;

            data = await UserService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task UserCreateDialog(int id, string title)
        {
            UserVM userData = await UserService.GetDetailsAsync(_httpClient, id);

            if (userData.InstructorTypeId == 0)
            {
                userData.InstructorTypeId = null;
            }

            if (userData.Gender != null)
            {
                userData.GenderId = userData.Gender == "Male" ? 0 : 1;
            }

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "userData", userData } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

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
                message = new NotificationMessage().Build(NotificationSeverity.Success, "User Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "User Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async Task UpdateIsUserActiveAsync(bool? value, int id)
        {
            CurrentResponse response = await UserService.UpdateIsUserActive(_httpClient, id, (bool)value);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async Task CloseUserStatusUpdateDialogAsync()
        {
            DialogService.Close(false);
            await grid.Reload();
        }
    }
}
