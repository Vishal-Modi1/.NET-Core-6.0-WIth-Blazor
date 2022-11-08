using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Aircraft.DetailsTabs.Discrepancy
{
    partial class DiscrepancyHistory
    {
        [Parameter] public long discrepancyId { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public List<DiscrepancyHistoryVM> discrepancyHistoryData { get; set; } = new();

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            discrepancyHistoryData = await DiscrepancyService.ListDiscrepancyHistoryAsync(dependecyParams, discrepancyId);

            ChangeLoaderVisibilityAction(false);
        }
    }
}

