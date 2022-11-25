using Web.UI.Utilities;
using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.JSInterop;

namespace Web.UI.Pages.Weather
{
    partial class Index
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
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            centers =  await AirTrafficControlCenterService.ListAllAsync(dependecyParams);

            dropDownValues = centers.Select(p => new DropDownValues() { Id = p.Id, Name = p.Name }).ToList();
        }

        public async Task LoadVideo(int value)
        {
            centerValue = value;
            selectedCenter = centers.Where(p => p.Id == value).FirstOrDefault();

            if(selectedCenter == null)
            {
                return;
            }

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("ReloadVideo");
        }
    }
}
