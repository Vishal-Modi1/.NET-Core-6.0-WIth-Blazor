using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Discrepancy
{
    partial class Create
    {
        [Parameter] public DiscrepancyVM discrepancyData { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        private async Task OnCompanyValueChange(int value)
        {
            if (value == 0)
            {
                return;
            }

            ChangeLoaderVisibilityAction(true);

            discrepancyData.CompanyId = value;
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

         //   discrepancyData.AircraftsList = await AircraftService.ListDropdownValuesByCompanyId(dependecyParams, discrepancyData.CompanyId);
            discrepancyData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, discrepancyData.CompanyId);

            ChangeLoaderVisibilityAction(false);
            base.StateHasChanged();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DiscperancyService.SaveandUpdateAsync(dependecyParams, discrepancyData);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }

            isBusySubmitButton = false;
        }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
