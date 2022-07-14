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
using DataModels.Enums;

namespace FSM.Blazor.Pages.UserRolePermission
{
    partial class Index
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        [CascadingParameter] public RadzenDataGrid<UserRolePermissionDataVM> grid { get; set; }
        [CascadingParameter] public RadzenDropDown<int> companyFilter { get; set; }
        [CascadingParameter] public RadzenDropDown<int> roleFilter { get; set; }
        [CascadingParameter] public RadzenDropDown<int> moduleFilter { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        IList<UserRolePermissionDataVM> data;
        UserRolePermissionFilterVM userrolePermissionFilterVM;
        IList<DropDownValues> CompanyFilterDropdown, RoleFilterDropdown, ModuleFilterDropdown;
        UserRolePermissionDataVM userRolePermissionDataVM;

        int CompanyId, RoleId, ModuleId, count;
        bool isLoading, isAllow, isAllowForMobileApp, isDisplayPopup, isForWebApp;
        string popupTitle, message;

        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = "UserRolePermission";

        OperationType operationType = OperationType.Create;

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
                await CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                NotificationService.Notify(message);
            }
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
                await CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                NotificationService.Notify(message);
            }
        }

        async Task RevokeUserStatusChange()
        {
            isDisplayPopup = false;
            data.Where(p => p.Id == userRolePermissionDataVM.Id).First().IsAllowed = !userRolePermissionDataVM.IsAllowed;
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
        }

        void OpenUpdateUserPermissionDialog(bool value, UserRolePermissionDataVM permissionData, bool isForWeb)
        {
            isDisplayPopup = true;
            operationType = OperationType.ActivateDeActivate;

            message = "Are you sure you want to grant the ";
            popupTitle = "Grant Permission";

            if (!isForWeb)
            {
                popupTitle = "Grant Mobile App Permission";
            }

            if (value == false)
            {
                message = "Are you sure you want to deny the ";
                popupTitle = "Deny Permission";

                if (!isForWeb)
                {
                    popupTitle = "Deny Mobile App Permission";
                }
            }

            isForWebApp = isForWeb;
            userRolePermissionDataVM = permissionData;
            userRolePermissionDataVM.IsAllowed = value;
        }

        void OpenUpdateUserPermissionsDialog(bool value, bool isForWeb)
        {
            isDisplayPopup = true;
            operationType = OperationType.ActivateDeActivateInBulk;

            message = "Are you sure you want to grant the permissions for all selected modules and roles ?";
            popupTitle = "Grant Permissions";

            if (!isForWeb)
            {
                popupTitle = "Grant Mobile App Permissions";
                isAllowForMobileApp = value;
            }
            else
            {
                isAllow = value;
            }

            if (value == false)
            {
                message = "Are you sure you want to deny the permissions for all selected modules and roles ?";
                popupTitle = "Deny Permission";

                if (!isForWeb)
                {
                    popupTitle = "Deny Mobile App Permission";
                }
            }

            isForWebApp = isForWeb;
        }
    }
}
