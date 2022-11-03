using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Discrepancy
{
    partial class CreateTabs
    {
        [Parameter] public DiscrepancyVM discrepancyData { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
