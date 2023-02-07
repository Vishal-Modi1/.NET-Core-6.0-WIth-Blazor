using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;
using DataModels.VM.Common;

namespace Web.UI.Pages.LogBook.CrewPassenger
{
    partial class Create
    {
        [Parameter] public CrewPassengerVM crewPassengerVM { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LogBookService.SaveandUpdateCrewPassengerAsync(dependecyParams, crewPassengerVM);

            isBusySubmitButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
