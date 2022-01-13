using DataModels.VM.Common;
using DataModels.VM.User;
using FSM.Blazor.Data.User;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using FSM.Blazor.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace FSM.Blazor.Pages.User
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<UserDataVM> grid { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> roleFilter { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        IList<UserDataVM> data;
        UserFilterVM userFilterVM;
        IList<DropDownValues> CompanyFilterDropdown;
        IList<DropDownValues> RoleFilterDropdown;
        int CompanyId, RoleId, count;

        // Loaders
        bool isLoading, isBusyAddNewButton, isBusyDeleteButton, isBusyUpdateStatusButton;

        #region Grid Variables

        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;

        #endregion

        string moduleName = "User";

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
         
            if(!_currentUserPermissionManager.IsAllowed(AuthStat,DataModels.Enums.PermissionType.View,moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

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
            pageSize = datatableParams.Length;

            data = await UserService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task UserCreateDialog(long id, string title)
        {
            SetAddNewButtonState(true);

            UserVM userData = await UserService.GetDetailsAsync(_httpClient, id);

            SetAddNewButtonState(false);

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

        async Task DeleteAsync(long id)
        {
            await SetDeleteButtonState(true);

            CurrentResponse response = await UserService.DeleteAsync(_httpClient, id);

            await SetDeleteButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
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

        async Task UpdateIsUserActiveAsync(bool? value, long id)
        {
            SetUpdateStatusButtonState(true);

            CurrentResponse response = await UserService.UpdateIsUserActive(_httpClient, id, value.GetValueOrDefault());

            SetUpdateStatusButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
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

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }

        private async Task SetDeleteButtonState(bool isBusy)
        {
            isBusyDeleteButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        private void SetUpdateStatusButtonState(bool isBusy)
        {
            isBusyUpdateStatusButton = isBusy;
            StateHasChanged();
        }
    }
}
