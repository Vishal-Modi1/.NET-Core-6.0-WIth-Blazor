using DataModels.Enums;
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
        DiscrepancyFileVM _discrepancy;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
        }

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
            _discrepancy = await DiscrepancyFileService.GetDetailsAsync(dependecyParams, discrepancyFileVM.Id);
            _discrepancy.DiscrepancyId = DiscrepancyIdParam;

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
            _discrepancy = data.First();
            operationType = OperationType.DocumentViewer;
            isDisplayChildPopup = true;
            childPopupTitle = "File";
            base.StateHasChanged();
        }
    }
}
