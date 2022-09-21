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
        [CascadingParameter] public TelerikGrid<UserRolePermissionDataVM> mobileAppPermissionGrid { get; set; }

        List<UserRolePermissionDataVM> data = new List<UserRolePermissionDataVM>();
        UserRolePermissionFilterVM userrolePermissionFilterVM;
        UserRolePermissionDataVM userRolePermissionDataVM;

        bool isAllow, isForWebApp, isAllowForMobileApp;
        string message;
        string moduleName = "UserRolePermission";
        IEnumerable<UserRolePermissionDataVM> webPermissions = new List<UserRolePermissionDataVM>();
        IEnumerable<UserRolePermissionDataVM> mobileAppPermissions = new List<UserRolePermissionDataVM>();
        TelerikTabStrip permissionsTabs;

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
            if (webPermissions.Count() > permissions.Count())
            {
                userRolePermissionDataVM = webPermissions.Where(p => !permissions.Contains(p)).First();
            }
            else
            {
                userRolePermissionDataVM = permissions.Where(p => !webPermissions.Contains(p)).First();
            }

            OpenUpdateUserPermissionDialog(!userRolePermissionDataVM.IsAllowed, userRolePermissionDataVM, true);
        }

        protected void OnMobilePermissionSelect(IEnumerable<UserRolePermissionDataVM> permissions)
        {
            if (mobileAppPermissions.Count() > permissions.Count())
            {
                userRolePermissionDataVM = mobileAppPermissions.Where(p => !permissions.Contains(p)).First();
            }
            else
            {
                userRolePermissionDataVM = permissions.Where(p => !mobileAppPermissions.Contains(p)).First();
            }

            OpenUpdateUserPermissionDialog(!userRolePermissionDataVM.IsAllowedForMobileApp, userRolePermissionDataVM, false);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            UserRolePermissionDatatableParams datatableParams = new DatatableParams().Create(args, "Name").Cast<UserRolePermissionDatatableParams>();

            datatableParams.CompanyId = userrolePermissionFilterVM.CompanyId;
            datatableParams.ModuleId = userrolePermissionFilterVM.ModuleId;
            datatableParams.RoleId = userrolePermissionFilterVM.UserRoleId;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await UserRolePermissionService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
            webPermissions = data.Where(p => p.IsAllowed).ToList();
            mobileAppPermissions =  data.Where(p => p.IsAllowedForMobileApp).ToList();

            isGridDataLoading = false;
        }

        bool? selectAllWebPermissions
        {
            get
            {
                if (IsAllWebPermissionSelected())
                {
                    return true;
                }
                else if (IsAnyWebPermissionSelected())
                {
                    return null;
                }

                return false;
            }

            set
            {
                if (value.HasValue && value.Value == true)
                {
                    webPermissions = data;
                }
                else
                {
                    webPermissions = new List<UserRolePermissionDataVM>();
                }
            }
        }

        bool? selectAllMobileAppPermissions
        {
            get
            {
                if (IsAllMobilePermissionSelected())
                {
                    return true;
                }
                else if (IsAnyMobileAppPermissionSelected())
                {
                    return null;
                }

                return false;
            }

            set
            {
                if (value.HasValue && value.Value == true)
                {
                    mobileAppPermissions = data;
                }
                else
                {
                    mobileAppPermissions = new List<UserRolePermissionDataVM>();
                }
            }
        }

        bool IsAnyWebPermissionSelected()
        {
            return grid.SelectedItems.Count() > 0 && grid.SelectedItems.Count() < data.Count() ;
        }

        bool IsAnyMobileAppPermissionSelected()
        {
            return mobileAppPermissionGrid.SelectedItems.Count() > 0 && mobileAppPermissionGrid.SelectedItems.Count() < data.Count();
        }

        bool IsAllWebPermissionSelected()
        {
            return grid.SelectedItems.Count() == data.Count();
        }

        bool IsAllMobilePermissionSelected()
        {
            return mobileAppPermissionGrid.SelectedItems.Count() == data.Count();
        }

        void SelectAllWebPermissions()
        {
            if (selectAllWebPermissions.HasValue && selectAllWebPermissions.Value)
            {
                selectAllWebPermissions = false;
                OpenUpdateUserPermissionsDialog(false, true);
            }
            else
            {
                selectAllWebPermissions = true;
                OpenUpdateUserPermissionsDialog(true, true);
            }
        }

        void SelectAllMobileAppPermissions()
        {
            if (selectAllMobileAppPermissions.HasValue && selectAllMobileAppPermissions.Value)
            {
                selectAllMobileAppPermissions = false;
                OpenUpdateUserPermissionsDialog(false, false);
            }
            else
            {
                selectAllMobileAppPermissions = true;
                OpenUpdateUserPermissionsDialog(true, false);
            }
        }

        private void OnCompanyValueChanges(int selectedValue)
        {
            if (userrolePermissionFilterVM.CompanyId != selectedValue)
            {
                RefreshGrid();
                userrolePermissionFilterVM.CompanyId = selectedValue;
            }
        }

        private void OnModuleValueChanges(int selectedValue)
        {
            if (userrolePermissionFilterVM.ModuleId != selectedValue)
            {
                RefreshGrid();
                userrolePermissionFilterVM.ModuleId = selectedValue;
            }
        }

        private void OnRoleValueChanges(int selectedValue)
        {
            if (userrolePermissionFilterVM.UserRoleId != selectedValue)
            {
                RefreshGrid();
                userrolePermissionFilterVM.UserRoleId = selectedValue;
            }
        }

        async Task UpdatePermissionAsync(bool? value, long id, bool isForWeb)
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserRolePermissionService.UpdatePermissionAsync(dependecyParams, id, value.GetValueOrDefault(), isForWeb);

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }

            isBusySubmitButton = false;
        }

        void CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                RefreshGrid();
            }
        }

        void RefreshGrid()
        {
            if (permissionsTabs.ActiveTabIndex == 0)
            {
                grid.Rebind();
            }
            else
            {
                mobileAppPermissionGrid.Rebind();
            }
        }

        async Task UpdatePermissionsAsync(bool value, bool isForWeb)
        {
            isBusySubmitButton = true;

            if (isForWeb)
            {
                userrolePermissionFilterVM.IsAllow = value;
            }
            else
            {
                userrolePermissionFilterVM.IsAllowForMobileApp = value;
            }

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

            isBusySubmitButton = false;
        }

        void RevokeUserPermissionStatusChange()
        {
            isDisplayPopup = false;
            RefreshGrid();

            data.Where(p => p.Id == userRolePermissionDataVM.Id).First().IsAllowed = !userRolePermissionDataVM.IsAllowed;
            //base.StateHasChanged();
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
