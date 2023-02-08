using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;
using DataModels.Enums;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook.Instrument
{
    partial class InstrumentApproach
    {
        [Parameter] public List<LogBookInstrumentApproachVM> logBookInstrumentApproachesVMList { get; set; }
        [Parameter] public List<DropDownSmallValues> Approaches { get; set; }

        LogBookInstrumentApproachVM instrumentApproach;

        protected override Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            return base.OnInitializedAsync();
        }

        async Task OpenDeleteDialog(LogBookInstrumentApproachVM selectedApproach)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Instrument Approach";

            instrumentApproach = selectedApproach;
        }

        async Task DeleteAsync()
        {
            if (instrumentApproach.Id > 0)
            {
                isBusyDeleteButton = true;
                   
                CurrentResponse response = await LogBookService.DeleteLogBookInstrumentApproachAsync(dependecyParams, instrumentApproach.Id);

                isBusyDeleteButton = false;

                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    isDisplayPopup = false;
                    logBookInstrumentApproachesVMList.Remove(instrumentApproach);
                    globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
                }
            }
            else
            {
                logBookInstrumentApproachesVMList.Remove(instrumentApproach);
                globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, "Passenger deleted successfully");
            }
        }
    }
}
