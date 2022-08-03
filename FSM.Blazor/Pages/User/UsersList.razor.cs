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
using DataModels.VM.Reservation;
using DataModels.Enums;

namespace FSM.Blazor.Pages.User
{
    partial class UsersList
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<UserDataVM> grid { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> roleFilter { get; set; }

        [Parameter]
        public string ParentModuleName { get; set; }

        [Parameter]
        public int? CompanyIdParam { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        IList<UserDataVM> data;
        UserFilterVM userFilterVM;
        IList<DropDownValues> CompanyFilterDropdown;
        IList<DropDownValues> RoleFilterDropdown;
        int CompanyId, RoleId, count;
        bool isDisplayPopup { get; set; }
        string popupTitle { get; set; }

        OperationType operationType = OperationType.Create;

        // Loaders
        bool isLoading, isBusyAddNewButton, isBusyDeleteButton, isBusyUpdateStatusButton;

        UserVM userData;
        #region Grid Variables

        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string message = "";

        #endregion

        string moduleName = "User";

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
         
            if(!_currentUserPermissionManager.IsAllowed(AuthStat,DataModels.Enums.PermissionType.View,moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            userFilterVM = await UserService.GetFiltersAsync(dependecyParams);
            CompanyFilterDropdown = userFilterVM.Companies;
            RoleFilterDropdown = userFilterVM.UserRoles;
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            UserDatatableParams datatableParams = new UserDatatableParams().Create(args, "StartDateTime");
          
            datatableParams.SearchText = searchText;
           
            pageSize = datatableParams.Length;

            if (ParentModuleName == Module.Company.ToString())
            {
                datatableParams.CompanyId = CompanyIdParam.GetValueOrDefault();
            }
            else
            {
                datatableParams.CompanyId = CompanyId;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await UserService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task UserCreateDialog(long id, string title)
        {
            if (id == 0)
            {
                operationType = OperationType.Create;
                SetAddNewButtonState(true);
            }
            else
            {
                operationType = OperationType.Edit;
                SetEditButtonState(id, true);
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            userData = await UserService.GetDetailsAsync(dependecyParams, id);

            SetAddNewButtonState(false);

            if (userData.InstructorTypeId == 0)
            {
                userData.InstructorTypeId = null;
            }

            if (userData.Gender != null)
            {
                userData.GenderId = userData.Gender == "Male" ? 0 : 1;
            }

            if (id == 0)
            {
                SetAddNewButtonState(false);
            }
            else
            {
                SetEditButtonState(id, false);
            }

            popupTitle = title;
            isDisplayPopup = true;
        }

        async Task DeleteAsync(long id)
        {
            await SetDeleteButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.DeleteAsync(dependecyParams, id);

            await SetDeleteButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                //DialogService.Close(true);
                isDisplayPopup = false;
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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await UserService.UpdateIsUserActive(dependecyParams, id, value.GetValueOrDefault());

            SetUpdateStatusButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
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

        async Task CloseUserStatusUpdateDialogAsync()
        {
            DialogService.Close(false);
            await grid.Reload();
        }

        async Task RevokeUserStatusChange()
        {
            isDisplayPopup = false;
            data.Where(p => p.Id == userData.Id).First().IsActive = !userData.IsActive;
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
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

        private void SetEditButtonState(long id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            details.IsLoadingEditButton = isBusy;
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

        void OpenUpdateUserStatusDialog(bool? value, UserDataVM userInfo)
        {
            isDisplayPopup = true;
            operationType = OperationType.ActivateDeActivate;

            message = "Are you sure, you want to activate ";
            popupTitle = "Actiavate User";

            userData = new UserVM();
            userData.Id = userInfo.Id;

            userData.FirstName = userInfo.FirstName;
            userData.LastName = userInfo.LastName;
            userData.IsActive = value.Value;

            if (value == false)
            {
                message = "Are you sure, you want to deactivate ";
                popupTitle = "Deactivate User";
            }
        }
    }
}
