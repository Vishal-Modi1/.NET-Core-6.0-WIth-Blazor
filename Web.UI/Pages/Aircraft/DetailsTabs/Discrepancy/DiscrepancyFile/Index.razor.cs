using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Aircraft.DetailsTabs.Discrepancy.DiscrepancyFile
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<DiscrepancyFileVM> grid { get; set; }
        [Parameter] public long DiscrepancyIdParam { get; set; }
        DiscrepancyFileVM _discrepancyFile;

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            var data = await DiscrepancyFileService.ListAsync(dependecyParams, DiscrepancyIdParam);

            args.Total = data.Count();
            args.Data = data;

            isGridDataLoading = false;
        }

        void CloseDialog(bool reloadGrid)
        {
            isDisplayChildPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
            }
        }

        async Task OpenDeleteDialog(DiscrepancyFileVM discrepancyFileVM)
        {
            isDisplayChildPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete File";

            _discrepancyFile = discrepancyFileVM;
        }

        async Task DeleteAsync(long id)
        {
            isBusyDeleteButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DiscrepancyFileService.DeleteAsync(dependecyParams, id);
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyDeleteButton = false;

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }
        }

        async Task OpenCreateDialog(DiscrepancyFileVM discrepancyFileVM)
        {
            if (discrepancyFileVM.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                childPopupTitle = "Create New Discrepancy";
            }
            else
            {
                operationType = OperationType.Edit;
                discrepancyFileVM.IsLoadingEditButton = true;
                childPopupTitle = "Update Discrepancy";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            _discrepancyFile = await DiscrepancyFileService.GetDetailsAsync(dependecyParams, discrepancyFileVM.Id);
            _discrepancyFile.DiscrepancyId = DiscrepancyIdParam;

            if (discrepancyFileVM.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                discrepancyFileVM.IsLoadingEditButton = false;
            }

            isDisplayChildPopup = true;
            base.StateHasChanged();
        }

        protected async void OnSelect(IEnumerable<DiscrepancyFileVM> data)
        {
            _discrepancyFile = data.First();
            operationType = OperationType.DocumentViewer;
            isDisplayChildPopup = true;
            childPopupTitle = "File";
            base.StateHasChanged();
        }
    }
}
