using Microsoft.AspNetCore.Components;

namespace Web.UI.Shared.Components.DialogFooter
{
    partial class FormFooterComponent
    {
        [Parameter] public long id { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public bool IsSubmitButtonLoading { get; set; }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
