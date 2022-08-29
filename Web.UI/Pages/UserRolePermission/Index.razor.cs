using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using Web.UI.Data.UserRolePermission;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;
using DataModels.Enums;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.UserRolePermission
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<UserRolePermissionDataVM> grid { get; set; }

        IList<UserRolePermissionDataVM> data;
        UserRolePermissionFilterVM userrolePermissionFilterVM;
        UserRolePermissionDataVM userRolePermissionDataVM;

        bool isAllow, isForWebApp, isAllowForMobileApp, IsInitialDataLoadComplete;
        string popupTitle, message;
        string moduleName = "UserRolePermission";
        IEnumerable<UserRolePermissionDataVM> selectedItems = new List<UserRolePermissionDataVM>();

        protected override async Task OnInitializedAsync()
        {
            userrolePermissionFilterVM = new UserRolePermissionFilterVM();
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            userrolePermissionFilterVM = await UserRolePermissionService.GetFiltersAsync(dependecyParams);
        }

        protected void OnSelect(IEnumerable<UserRolePermissionDataVM> permissions)
        {
            selectedItems = permissions;
        }

        async Task LoadData(GridReadEventArgs args)
        {
            IsInitialDataLoadComplete = true;
            UserRolePermissionDatatableParams datatableParams = new DatatableParams().Create(args, "Name").Cast<UserRolePermissionDatatableParams>();

            datatableParams.CompanyId = userrolePermissionFilterVM.CompanyId;
            datatableParams.ModuleId = userrolePermissionFilterVM.ModuleId;
            datatableParams.RoleId = userrolePermissionFilterVM.UserRoleId;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await UserRolePermissionService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
            selectedItems = data.Where(p => p.IsAllowed).ToList();

            IsInitialDataLoadComplete = false;
        }

        private void OnCompanyValueChanges(int selectedValue)
        {
            if (userrolePermissionFilterVM.CompanyId != selectedValue)
            {
                grid.Rebind();
                userrolePermissionFilterVM.CompanyId = selectedValue;
            }
        }

        private void OnModuleValueChanges(int selectedValue)
        {
            if (userrolePermissionFilterVM.ModuleId != selectedValue)
            {
                grid.Rebind();
                userrolePermissionFilterVM.ModuleId = selectedValue;
            }
        }

        private void OnRoleValueChanges(int selectedValue)
        {
            if (userrolePermissionFilterVM.UserRoleId != selectedValue)
            {
                grid.Rebind();
                userrolePermissionFilterVM.UserRoleId = selectedValue;
            }
        }

        async Task UpdatePermissionAsync(bool? value, long id, bool isForWeb)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserRolePermissionService.UpdatePermissionAsync(dependecyParams, id, value.GetValueOrDefault(), isForWeb);

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }
        }

        async Task CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
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

            //userrolePermissionFilterVM.CompanyId = CompanyId;
            //userrolePermissionFilterVM.ModuleId = ModuleId;
            //userrolePermissionFilterVM.UserRoleId = RoleId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserRolePermissionService.UpdatePermissionsAsync(dependecyParams, userrolePermissionFilterVM, isForWeb);

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }
        }

        async Task RevokeUserStatusChange()
        {
            isDisplayPopup = false;
            data.Where(p => p.Id == userRolePermissionDataVM.Id).First().IsAllowed = !userRolePermissionDataVM.IsAllowed;
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

            userRolePermissionDataVM = new UserRolePermissionDataVM();
            userRolePermissionDataVM.IsAllowed = value;

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
