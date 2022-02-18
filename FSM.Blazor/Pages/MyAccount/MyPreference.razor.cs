using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;
using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Pages.MyAccount
{
    partial class MyPreference
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        UserPreferenceVM userPreferenceVM = new UserPreferenceVM();
        private bool isDisplayLoader { get; set; } = false;

        bool isBusy;
        List<DropDownValues> preferenceTypesList = new List<DropDownValues>();
        List<DropDownLargeValues> aircraftList = new List<DropDownLargeValues>();
        List<DropDownValues> activityTypeList = new List<DropDownValues>();

        protected override void OnInitialized()
        {
            int i = 0;
            foreach (var item in Enum.GetNames(typeof(PreferenceType)).ToList())
            {
                i++;
                preferenceTypesList.Add(new DropDownValues() { Id = i, Name = item });
            }

            base.OnInitialized();
        }

        public async Task GetPreferenceValues(object values)
        {
            if(values.ToString() == PreferenceType.ScheduleActivityType.ToString())
            {
                activityTypeList = await AircraftSchedulerService.ListActivityTypeDropDownValues(_httpClient);
            }
            else if(values.ToString() == PreferenceType.Aircraft.ToString())
            {
                aircraftList = await AircraftService.ListDropdownValues(_httpClient);
            }
        }

        public async Task Submit()
        {

        }
    }
}
