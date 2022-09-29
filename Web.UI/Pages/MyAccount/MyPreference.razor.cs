using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Web.UI.Utilities;

namespace Web.UI.Pages.MyAccount
{
    partial class MyPreference
    {
        [Parameter]
        public List<UserPreferenceVM> UserPreferencesList { get; set; }
        UserPreferenceVM userPreferenceVM = new UserPreferenceVM();

        EditContext preferenceForm;

        protected override async Task OnInitializedAsync()
        {
            preferenceForm = new EditContext(userPreferenceVM);

            int i = 0;

            userPreferenceVM.PreferenceTypesList = new List<DropDownValues>();

            foreach (var item in Enum.GetNames(typeof(PreferenceType)).ToList())
            {
                i++;
                userPreferenceVM.PreferenceTypesList.Add(new DropDownValues() { Id = i, Name = item });
            }

            base.OnInitialized();
        }

        void UpdateSelectedAircraftData(List<long> selectedData)
        {
            userPreferenceVM.AircraftIds = selectedData;
        }

        void UpdateSelectedActivityTypeData(List<long> selectedData)
        {
            userPreferenceVM.ActivityIds = selectedData;
        }

        public async Task GetPreferenceValues(int values)
        {
            ChangeLoaderVisibilityAction(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            userPreferenceVM.PreferenceTypeId = values;
            userPreferenceVM.ActivityTypeList = await AircraftSchedulerService.ListActivityTypeDropDownValues(dependecyParams);
            var activityTypeData = UserPreferencesList.Where(p => p.PreferenceType == PreferenceType.ScheduleActivityType).FirstOrDefault();

            if (activityTypeData != null)
            {
                userPreferenceVM.ActivityIds = userPreferenceVM.ActivityTypeList.Where(p => activityTypeData.ListPreferencesIds.Contains(p.Id.ToString())).Select(p => Convert.ToInt64(p.Id)).ToList();
            }

            userPreferenceVM.AircraftList = await AircraftService.ListDropdownValues(dependecyParams);
            var aircraftData = UserPreferencesList.Where(p => p.PreferenceType == PreferenceType.Aircraft).FirstOrDefault();

            if (aircraftData != null)
            {
                userPreferenceVM.AircraftIds = userPreferenceVM.AircraftList.Where(p => aircraftData.ListPreferencesIds.Contains(p.Id.ToString())).Select(p => p.Id).ToList();
            }

            preferenceForm.Validate();
            ChangeLoaderVisibilityAction(false);
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;
            base.StateHasChanged();

            userPreferenceVM.PreferenceType = (PreferenceType)userPreferenceVM.PreferenceTypeId;
            userPreferenceVM.ListPreferencesIds = new List<string>();

            if (userPreferenceVM.PreferenceTypeId == (int)PreferenceType.Aircraft)
            {
                foreach (long value in userPreferenceVM.AircraftIds)
                {
                    var aircraft = userPreferenceVM.AircraftList.Where(p => p.Id == value).FirstOrDefault();

                    if (aircraft == null)
                    {
                        continue;
                    }

                    userPreferenceVM.ListPreferencesIds.Add(aircraft.Id.ToString());
                }
            }
            else
            {
                foreach (long value in userPreferenceVM.ActivityIds)
                {

                    var activityType = userPreferenceVM.ActivityTypeList.Where(p => p.Id == value).FirstOrDefault();

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

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
        }
    }
}
