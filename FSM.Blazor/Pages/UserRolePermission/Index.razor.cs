﻿using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using FSM.Blazor.Data.UserRolePermission;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.UserRolePermission
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<UserRolePermissionDataVM> grid { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> roleFilter { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> moduleFilter { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        IList<UserRolePermissionDataVM> data;
        UserRolePermissionFilterVM userrolePermissionFilterVM;
        IList<DropDownValues> CompanyFilterDropdown, RoleFilterDropdown, ModuleFilterDropdown;
        int CompanyId, RoleId, ModuleId, count;
        bool isLoading, isAllow;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;

        protected override async Task OnInitializedAsync()
        {
            userrolePermissionFilterVM = await UserRolePermissionService.GetFiltersAsync(_httpClient);
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

            data = await UserRolePermissionService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task UpdateUserPermissionAsync(bool? value, int id)
        {
            CurrentResponse response = await UserRolePermissionService.UpdatePermissionAsync(_httpClient, id, (bool)value);

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

        async Task UpdateUserMultiplePermissionAsync(bool value)
        {
            userrolePermissionFilterVM.IsAllow = value;
            userrolePermissionFilterVM.CompanyId = CompanyId;
            userrolePermissionFilterVM.ModuleId = ModuleId;
            userrolePermissionFilterVM.UserRoleId = RoleId;

            CurrentResponse response = await UserRolePermissionService.UpdateMultiplePermissionsAsync(_httpClient, userrolePermissionFilterVM);

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
