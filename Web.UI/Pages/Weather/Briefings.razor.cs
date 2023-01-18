using Web.UI.Utilities;
using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.JSInterop;

namespace Web.UI.Pages.Weather
{
    partial class Briefings
    {
        List<AirTrafficControlCenter> centers = new();
        List<DropDownValues> dropDownValues = new();
        int centerValue;
        AirTrafficControlCenter selectedCenter;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            centers = await AirTrafficControlCenterService.ListAllAsync(dependecyParams);

            dropDownValues = centers.Select(p => new DropDownValues() { Id = p.Id, Name = p.Name }).ToList();

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            centerValue = await AirTrafficControlCenterService.GetDefault(dependecyParams);

            if (centerValue != 0)
            {
                selectedCenter = centers.Where(p => p.Id == centerValue).FirstOrDefault();
            }

            ChangeLoaderVisibilityAction(false);
        }

        public async Task LoadVideo(int value)
        {
            centerValue = value;
            selectedCenter = centers.Where(p => p.Id == value).FirstOrDefault();

            if (selectedCenter == null)
            {
                return;
            }

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("ReloadVideo");
        }

        public async Task SetDefault()
        {
            if (centerValue == 0)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Please select center");
                return;
            }

            isBusyAddButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AirTrafficControlCenterService.SetDefault(dependecyParams, centerValue);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusyAddButton = false;
        }

        private async Task RedirectToNewTab(string link)
        {
            await JSRuntime.InvokeVoidAsync("open", link, "_blank");
        }
    }
}
