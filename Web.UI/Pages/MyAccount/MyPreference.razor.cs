using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Web.UI.Utilities;

namespace Web.UI.Pages.MyAccount
{
    partial class MyPreference
    {
        [Parameter]
        public List<UserPreferenceVM> UserPreferencesList { get; set; }

        UserPreferenceVM userPreferenceVM = new UserPreferenceVM();
        List<DropDownValues> preferenceTypesList = new List<DropDownValues>();
        List<DropDownLargeValues> aircraftList = new List<DropDownLargeValues>();
        List<DropDownValues> activityTypeList = new List<DropDownValues>();

        [Required(ErrorMessage = "Preference type is required")]
        int preferenceTypeId = 0;

        //[Required(ErrorMessage = "Aircraft is required")]
        List<long> multipleValues = new List<long>();
        List<int> multipleActivites = new List<int>();

        protected override async Task OnInitializedAsync()
        {
            int i = 0;
            foreach (var item in Enum.GetNames(typeof(PreferenceType)).ToList())
            {
                i++;
                preferenceTypesList.Add(new DropDownValues() { Id = i, Name = item });
            }

            base.OnInitialized();
        }

        public async Task GetPreferenceValues(int values)
        {
            isDisplayLoader = true;

            activityTypeList = new List<DropDownValues>();
            aircraftList = new List<DropDownLargeValues>();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            preferenceTypeId = values;
            if (values == (int)PreferenceType.ScheduleActivityType)
            {
                activityTypeList = await AircraftSchedulerService.ListActivityTypeDropDownValues(dependecyParams);
                var activityTypeData = UserPreferencesList.Where(p => p.PreferenceType == PreferenceType.ScheduleActivityType).FirstOrDefault();

                if (activityTypeData != null)
                {
                    multipleActivites = activityTypeList.Where(p => activityTypeData.ListPreferencesIds.Contains(p.Id.ToString())).Select(p => Convert.ToInt32(p.Id)).ToList();
                }
            }
            else if (values == (int)PreferenceType.Aircraft)
            {
                aircraftList = await AircraftService.ListDropdownValues(dependecyParams);
                var aircraftData = UserPreferencesList.Where(p => p.PreferenceType == PreferenceType.Aircraft).FirstOrDefault();

                if (aircraftData != null)
                {
                    multipleValues = aircraftList.Where(p => aircraftData.ListPreferencesIds.Contains(p.Id.ToString())).Select(p=>p.Id).ToList();
                }
            }

            isDisplayLoader = false;
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;
            base.StateHasChanged();

            userPreferenceVM.PreferenceType = (PreferenceType)preferenceTypeId;
            userPreferenceVM.ListPreferencesIds = new List<string>();

            if (preferenceTypeId == (int)PreferenceType.Aircraft)
            {
                foreach (long value in multipleValues)
                {
                    var aircraft = aircraftList.Where(p=>p.Id == value).FirstOrDefault();

                    if(aircraft == null)
                    {
                        continue;
                    }

                    userPreferenceVM.ListPreferencesIds.Add(aircraft.Id.ToString());
                }
            }
            else
            {
                foreach (long value in multipleActivites)
                {

                    var activityType = activityTypeList.Where(p => p.Id == value).FirstOrDefault();

                    if (activityType == null)
                    {
                        continue;
                    }

                    userPreferenceVM.ListPreferencesIds.Add(activityType.Id.ToString());
                }
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await MyAccountService.AddMyPreference(dependecyParams, userPreferenceVM);

            isBusySubmitButton = false;

            uiNotification.DisplayNotification(uiNotification.Instance, response);
        }
    }
}
