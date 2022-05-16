using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using FSM.Blazor.Data.UserRolePermission;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using FSM.Blazor.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace FSM.Blazor.Pages.UserRolePermission
{
    partial class Index
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<UserRolePermissionDataVM> grid { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> roleFilter { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> moduleFilter { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        IList<UserRolePermissionDataVM> data;
        UserRolePermissionFilterVM userrolePermissionFilterVM;
        IList<DropDownValues> CompanyFilterDropdown, RoleFilterDropdown, ModuleFilterDropdown;
        int CompanyId, RoleId, ModuleId, count;
        bool isLoading, isAllow, isAllowForMobileApp;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = "UserRolePermission";


        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            userrolePermissionFilterVM = await UserRolePermissionService.GetFiltersAsync(dependecyParams);
            CompanyFilterDropdown = userrolePermissionFilterVM.Companies;
            RoleFilterDropdown = userrolePermissionFilterVM.UserRoles;
            ModuleFilterDropdown = userrolePermissionFilterVM.ModuleList;
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            UserRolePermissionDatatableParams datatableParams = new UserRolePermissionDatatableParams().Create(args, "Name");

            datatableParams.CompanyId = CompanyId;
            datatableParams.ModuleId = ModuleId;
            datatableParams.RoleId = RoleId;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await UserRolePermissionService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task UpdatePermissionAsync(bool? value, long id, bool isForWeb)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await UserRolePermissionService.UpdatePermissionAsync(dependecyParams, id, value.GetValueOrDefault(), isForWeb);

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

        async Task UpdatePermissionsAsync(bool value, bool isForWeb)
        {
            if (isForWeb)
            {
                userrolePermissionFilterVM.IsAllow = value;
            }
            else
            {
                userrolePermissionFilterVM.IsAllowForMobileApp = value;
            }
            userrolePermissionFilterVM.CompanyId = CompanyId;
            userrolePermissionFilterVM.ModuleId = ModuleId;
            userrolePermissionFilterVM.UserRoleId = RoleId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await UserRolePermissionService.UpdatePermissionsAsync(dependecyParams, userrolePermissionFilterVM, isForWeb);

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

        async Task CloseUserPermissionUpdateDialogAsync()
        {
            DialogService.Close(false);
            await grid.Reload();
        }
    }
}
