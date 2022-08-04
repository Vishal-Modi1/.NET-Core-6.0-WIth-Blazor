using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FSM.Blazor.Pages.MyAccount
{
    partial class ChangeCompany
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public List<DropDownValues> CompanyList { get; set; }
        [Parameter] public int CompanyId { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        
        public bool isBusy = false;

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        public async Task Update(int companyId)
        {
            isBusy = true;

            var authModule = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("ChangeCompany",Convert.ToInt64(Id), CompanyId);

            this.StateHasChanged();

            isBusy = false;
        }
    }
}
