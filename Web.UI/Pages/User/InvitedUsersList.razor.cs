﻿using DataModels.VM.Common;
using DataModels.VM.User;
using Web.UI.Data.User;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;
using DataModels.VM.Reservation;
using DataModels.Enums;
using Telerik.Blazor.Components;
using System.Security.Claims;

namespace Web.UI.Pages.User
{
    partial class InvitedUsersList
    {
        [CascadingParameter] public TelerikGrid<InviteUserDataVM> grid { get; set; }
        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyIdParam { get; set; }

        IList<InviteUserDataVM> data;
        UserFilterVM userFilterVM;
        InviteUserVM userData;

        string moduleName = "User";
        protected override async Task OnInitializedAsync()
        {
            userFilterVM = new UserFilterVM();
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
         
            if(!_currentUserPermissionManager.IsAllowed(AuthStat,PermissionType.View,moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            userFilterVM = await UserService.GetFiltersAsync(dependecyParams);
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

            if (userFilterVM.RoleId != 0)
            {
                datatableParams.RoleId = userFilterVM.RoleId;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await InviteUserService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            isGridDataLoading = false;
        }

        async Task InviteUserDialog(InviteUserDataVM inviteUserDataVM)
        {
            if (inviteUserDataVM.Id == 0)
            {
                operationType = OperationType.Create;
                popupTitle = "Invite User";
                isBusyAddButton = true;
            }
            else
            {
                operationType = OperationType.Edit;
                inviteUserDataVM.IsLoadingEditButton = true;
                popupTitle = "Update Invited User Details";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            userData = await InviteUserService.GetDetailsAsync(dependecyParams, inviteUserDataVM.Id);

            if(globalMembers.UserRole != UserRole.SuperAdmin)
            {
                userData.CompanyId = globalMembers.CompanyId;
            }

            if (inviteUserDataVM.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                inviteUserDataVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
        }

        async Task DeleteAsync(long id)
        {
            isBusyDeleteButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InviteUserService.DeleteAsync(dependecyParams, id);

            isBusyDeleteButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                await CloseDialog(true);
            }
            else
            {
                await CloseDialog(false);
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

        async Task OpenDeleteDialog(InviteUserDataVM userInfo)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete User";

            userData = new InviteUserVM();
            userData.Id = userInfo.Id;

            userData.Email = userInfo.Email;
        }

        private void OnCompanyValueChanges(int selectedValue)
        {
            if (userFilterVM.CompanyId != selectedValue)
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
    }
}
