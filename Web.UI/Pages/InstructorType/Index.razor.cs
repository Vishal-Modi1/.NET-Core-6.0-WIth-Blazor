using DataModels.VM.Common;
using DataModels.VM.InstructorType;
using Web.UI.Data.InstructorType;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using DataModels.Enums;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.InstructorType
{
    partial class Index
    {
        bool isFilterBarVisible;
        [CascadingParameter] public TelerikGrid<InstructorTypeVM> grid { get; set; }
        IList<InstructorTypeVM> data;
        string moduleName = "InstructorType";
        private InstructorTypeVM _instructorTypeData;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");
            pageSize = datatableParams.Length;
            datatableParams.SearchText = searchText;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await InstructorTypeService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        async Task InstructorTypeCreateDialog(InstructorTypeVM instructorTypeData)
        {
            if (instructorTypeData.Id == 0)
            {
                operationType = OperationType.Create;
                popupTitle = "Create Instructor Type";
            }
            else
            {
                operationType = OperationType.Edit;
                popupTitle = "Update Instructor Type";
            }

            _instructorTypeData = instructorTypeData;
            isDisplayPopup = true;
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InstructorTypeService.DeleteAsync(dependecyParams, id);

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

        async Task OpenDeleteDialog(InstructorTypeVM instructorTypeVM)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _instructorTypeData = instructorTypeVM;
            popupTitle = "Delete Instructor Type";
        }
    }
}
