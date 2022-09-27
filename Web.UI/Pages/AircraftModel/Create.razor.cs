using DataModels.VM.Common;
using DE = DataModels.Entities;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.AircraftModel
{
    public partial class Create
    {
        [Parameter] public DE.AircraftModel aircraftModel { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftModelService.SaveandUpdateAsync(dependecyParams, aircraftModel);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }

            isBusySubmitButton = false;
        }
        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
