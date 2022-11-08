using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Pages.Aircraft.DetailsTabs.Discrepancy
{
    partial class Create
    {
        [Parameter] public DiscrepancyVM discrepancyData { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public EventCallback<DiscrepancyVM> UpdateTabUI { get; set; }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DiscrepancyService.SaveandUpdateAsync(dependecyParams, discrepancyData);

            DiscrepancyVM discrepancyInfo = JsonConvert.DeserializeObject<DiscrepancyVM>(response.Data.ToString());
            
            discrepancyData.Id = discrepancyInfo.Id;
            discrepancyData.DiscrepancyHistoryVM = discrepancyInfo.DiscrepancyHistoryVM;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
            isBusySubmitButton = false;

            UpdateTab();
        }

        public void CloseDialog()
        {
            CloseDialogCallBack.InvokeAsync(true);
        }

        public void UpdateTab()
        {
            UpdateTabUI.InvokeAsync(discrepancyData);
        }
    }
}
