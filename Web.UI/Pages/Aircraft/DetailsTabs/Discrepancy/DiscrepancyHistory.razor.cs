using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Aircraft.DetailsTabs.Discrepancy
{
    partial class DiscrepancyHistory
    {
        [Parameter] public List<DiscrepancyHistoryVM> discrepancyHistoryData { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
