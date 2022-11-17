using DataModels.VM.Common;
using DataModels.VM.User;
using Web.UI.Data.User;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;
using DataModels.Enums;
using Telerik.Blazor.Components;
using System.Security.Claims;

namespace Web.UI.Pages.User
{
    partial class UsersList
    {
        [CascadingParameter] public TelerikGrid<UserDataVM> grid { get; set; }
        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyIdParam { get; set; }

        UserFilterVM userFilterVM;

        UserVM userData;
        string message = "", moduleName = "User";
        bool isBusyUpdateStatusButton;
        List<UserDataVM> data;
    
        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            userFilterVM = new UserFilterVM();

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            userFilterVM = await UserService.GetFiltersAsync(dependecyParams);

          }

        private void OnCompanyValueChanges(int selectedValue)
        {
            if(userFilterVM.CompanyId != selectedValue)
            {
                grid.Rebind();
                userFilterVM.CompanyId = selectedValue;
            }
        }

        private void OnRoleValueChanges(int selectedValue)
        {
            if (userFilterVM.RoleId != selectedValue)
            {
                grid.Rebind();
                userFilterVM.RoleId = selectedValue;
            }
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            UserDatatableParams datatableParams = new DatatableParams().Create(args, "StartDateTime").Cast<UserDatatableParams>();
            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            if (ParentModuleName == Module.Company.ToString())
            {
                datatableParams.CompanyId = CompanyIdParam.GetValueOrDefault();
            }
            else
            {
                datatableParams.CompanyId = userFilterVM.CompanyId;
            }

            if(userFilterVM.RoleId != 0)
            {
                datatableParams.RoleId = userFilterVM.RoleId;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await UserService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            isGridDataLoading = false;
        }

        async Task UserCreateDialog(UserDataVM userInfo)
        {
            if (userInfo.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Create User";
            }
            else
            {
                operationType = OperationType.Edit;
                userInfo.IsLoadingEditButton = true;
                popupTitle = "Update User Details";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            userData = await UserService.GetDetailsAsync(dependecyParams, userInfo.Id, userInfo.CompanyId.GetValueOrDefault());

            if (userData.InstructorTypeId == 0)
            {
                userData.InstructorTypeId = null;
            }

            if (userData.Gender != null)
            {
                userData.GenderId = userData.Gender == "Male" ? 0 : 1;
            }

            if (userInfo.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                userInfo.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
        }

        async Task DeleteAsync(long id)
        {
            isBusyDeleteButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.DeleteAsync(dependecyParams, id);

            isBusyDeleteButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
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

        async Task UpdateIsUserActiveAsync(bool? value, long id)
        {
            isBusyUpdateStatusButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await UserService.UpdateIsUserActive(dependecyParams, id, value.GetValueOrDefault());

            isBusyUpdateStatusButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

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
            data.Where(p => p.Id == userData.Id).First().IsActive = !userData.IsActive;
        }

        async Task OpenDeleteDialog(UserDataVM userInfo)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete User";

            userData = new UserVM();
            userData.Id = userInfo.Id;

            userData.FirstName = userInfo.FirstName;
            userData.LastName = userInfo.LastName;
        }

        void OpenUpdateUserStatusDialog(UserDataVM userInfo)
        {
            isDisplayPopup = true;
            operationType = OperationType.ActivateDeActivate;

            message = "Are you sure, you want to activate ";
            popupTitle = "Actiavate User";

            userData = new UserVM();
            userData.Id = userInfo.Id;

            userData.FirstName = userInfo.FirstName;
            userData.LastName = userInfo.LastName;
            userData.IsActive = userInfo.IsActive;

            if (userData.IsActive == false)
            {
                message = "Are you sure, you want to deactivate ";
                popupTitle = "Deactivate User";
            }
        }
    }
}
