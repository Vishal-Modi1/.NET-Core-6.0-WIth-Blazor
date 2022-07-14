using DataModels.VM.Common;
using DataModels.VM.InstructorType;
using FSM.Blazor.Data.InstructorType;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using DataModels.Enums;

namespace FSM.Blazor.Pages.InstructorType
{
    partial class Index
    {
        #region Params

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<InstructorTypeVM> grid { get; set; }


        private CurrentUserPermissionManager _currentUserPermissionManager;

        IList<InstructorTypeVM> data;
        int count;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        bool isLoading;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string searchText;
        string moduleName = "InstructorType";

        bool isDisplayPopup { get; set; }
        string popupTitle { get; set; }
        private InstructorTypeVM _instructorTypeData;

        OperationType operationType = OperationType.Create;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");
            pageSize = datatableParams.Length;
            datatableParams.SearchText = searchText;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await InstructorTypeService.ListAsync(dependecyParams,datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;            
        }

        async Task InstructorTypeCreateDialog(InstructorTypeVM instructorTypeData, string title)
        {
            if (instructorTypeData.Id == 0)
            {
                operationType = OperationType.Create;
            }
            else
            {
                operationType = OperationType.Edit;
            }

            _instructorTypeData = instructorTypeData;
            popupTitle = title;
            isDisplayPopup = true;
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            CurrentResponse response = await InstructorTypeService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                await CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "InstructorType Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "InstructorType Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
        }

        async Task OpenDeleteDialog(InstructorTypeVM instructorTypeVM)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _instructorTypeData = instructorTypeVM;
            popupTitle = "Delete Instructor Type";
        }
    }
}
