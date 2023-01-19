using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Reservation.FlightTag
{
    public partial class Create
    {
        FlightTagVM flightTagVM = new FlightTagVM();

        [Parameter]
        public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await FlightTagService.SaveTagAsync(dependecyParams, flightTagVM);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        public void CloseDialog(bool reloadList)
        {
            CloseDialogCallBack.InvokeAsync(reloadList);
        }
    }
}
