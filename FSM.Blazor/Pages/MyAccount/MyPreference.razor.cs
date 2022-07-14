using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;
using Microsoft.AspNetCore.Components;
using Radzen;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;

namespace FSM.Blazor.Pages.MyAccount
{
    partial class MyPreference
    {
        [Parameter]
        public List<UserPreferenceVM> UserPreferencesList { get; set; }

        UserPreferenceVM userPreferenceVM = new UserPreferenceVM();
        private bool isDisplayLoader { get; set; } = false;

        bool isBusy;
        List<DropDownValues> preferenceTypesList = new List<DropDownValues>();
        List<DropDownLargeValues> aircraftList = new List<DropDownLargeValues>();
        List<DropDownValues> activityTypeList = new List<DropDownValues>();
        int preferenceTypeId = 0;
        IEnumerable<string> multipleValues = new string[] { "" };

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

        public async Task GetPreferenceValues(object values)
        {
            isDisplayLoader = true;

            activityTypeList = new List<DropDownValues>();
            aircraftList = new List<DropDownLargeValues>();

            if (values == null)
            {
                return;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            if ((int)values == (int)PreferenceType.ScheduleActivityType)
            {
                activityTypeList = await AircraftSchedulerService.ListActivityTypeDropDownValues(dependecyParams);
                var activityTypeData = UserPreferencesList.Where(p => p.PreferenceType == PreferenceType.ScheduleActivityType).FirstOrDefault();

                if (activityTypeData != null)
                {
                    multipleValues = activityTypeList.Where(p => activityTypeData.ListPreferencesIds.Contains(p.Id.ToString())).Select(p => p.Name);
                }
            }
            else if ((int)values == (int)PreferenceType.Aircraft)
            {
                aircraftList = await AircraftService.ListDropdownValues(dependecyParams);
                var aircraftData = UserPreferencesList.Where(p => p.PreferenceType == PreferenceType.Aircraft).FirstOrDefault();

                if (aircraftData != null)
                {
                    multipleValues = aircraftList.Where(p => aircraftData.ListPreferencesIds.Contains(p.Id.ToString())).Select(p=>p.Name);
                }
            }

            isDisplayLoader = false;
        }

        private void ManageResponse(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, "User Preference", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong!", response.Message);
                NotificationService.Notify(message);
            }
        }

        public async Task Submit()
        {
            isBusy = true;
            base.StateHasChanged();

            userPreferenceVM.PreferenceType = (PreferenceType)preferenceTypeId;
            userPreferenceVM.ListPreferencesIds = new List<string>();

            if (preferenceTypeId == (int)PreferenceType.Aircraft)
            {
                foreach (string value in multipleValues)
                {
                    var aircraft = aircraftList.Where(p=>p.Name == value).FirstOrDefault();

                    if(aircraft == null)
                    {
                        continue;
                    }

                    userPreferenceVM.ListPreferencesIds.Add(aircraft.Id.ToString());
                }
            }
            else
            {
                foreach (string value in multipleValues)
                {

                    var activityType = activityTypeList.Where(p => p.Name == value).FirstOrDefault();

                    if (activityType == null)
                    {
                        continue;
                    }

                    userPreferenceVM.ListPreferencesIds.Add(activityType.Id.ToString());
                }
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await MyAccountService.AddMyPreference(dependecyParams, userPreferenceVM);

            isBusy = false;

            ManageResponse(response);

            base.StateHasChanged();
        }
    }
}
