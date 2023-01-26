using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Reservation.FlightTag
{
    public partial class Create
    {
        FlightTagVM flightTagVM = new FlightTagVM();
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public int CompanyId { get; set; }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            flightTagVM.CompanyId = CompanyId;
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await FlightTagService.SaveTagAsync(dependecyParams, flightTagVM);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }

            isBusySubmitButton = false;
        }

        public void CloseDialog(bool reloadList)
        {
            CloseDialogCallBack.InvokeAsync(reloadList);
        }
    }
}
