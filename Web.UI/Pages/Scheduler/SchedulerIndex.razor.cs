using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using DataModels.VM.UserPreference;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Utilities;
using DE = DataModels.Entities;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Web.UI.Data.AircraftSchedule;

namespace Web.UI.Pages.Scheduler
{
    partial class SchedulerIndex
    {
        public TelerikScheduler<SchedulerVM> scheduleRef { get; set; }

        SchedulerVM schedulerVM;
        List<SchedulerVM> dataSource;
        public SchedulerView currentView { get; set; } = SchedulerView.Week;
        public List<string> resources = new List<string>() { "AircraftId" };
        List<ResourceData> aircraftsResourceList = new List<ResourceData>();
        string moduleName = "Scheduler";
        public UIOptions uiOptions = new UIOptions();
        string timezone = "";
        SchedulerFilter schedulerFilter = new SchedulerFilter();
        List<DE.Aircraft> allAircraftList = new List<DE.Aircraft>();
        DependecyParams dependecyParams;

        int multiDayDaysCount { get; set; } = 10;
        DateTime currentDate = DateTime.Now;

        List<long> multipleAircrafts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            dataSource = new List<SchedulerVM>();
            timezone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);
            currentDate = DateConverter.ToLocal(DateTime.UtcNow, timezone);
            
            InitializeValues();

            schedulerVM = new SchedulerVM();
            schedulerVM.ScheduleActivitiesList = new List<DropDownValues>();
            aircraftsResourceList = new List<ResourceData>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
               
                if (!globalMembers.IsSuperAdmin)
                {
                    List<UserPreferenceVM> userPrefernecesList = await UserService.FindMyPreferencesById(dependecyParams);
                    UserPreferenceVM aircraftPreference = userPrefernecesList.Where(p => p.PreferenceType == PreferenceType.Aircraft).FirstOrDefault();

                    aircraftsResourceList = await GetAircraftData(aircraftPreference);
                }
                else
                {
                    schedulerFilter.Companies = await CompanyService.ListDropDownValues(dependecyParams);
                }

                await LoadDataAsync();

                ChangeLoaderVisibilityAction(false);
                base.StateHasChanged();
            }
        }

        async Task ViewChangedHandler(SchedulerView nextView)
        {
            currentView = nextView;
        }

        async Task DateChangedHandler(DateTime currDate)
        {
            currentDate = currDate;
            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            ChangeLoaderVisibilityAction(true);

            Tuple<DateTime, DateTime> dates = TelerikSchedulerDateHelper.GetDates(currentDate, currentView, multiDayDaysCount);

            schedulerFilter.StartTime = dates.Item1;
            schedulerFilter.EndTime = dates.Item2;

            schedulerFilter.StartTime = DateConverter.ToUTC(schedulerFilter.StartTime.Date, timezone);
            schedulerFilter.EndTime = DateConverter.ToUTC(schedulerFilter.EndTime.Date.AddDays(1).AddTicks(-1), timezone);

            dataSource = await AircraftSchedulerService.ListAsync(dependecyParams, schedulerFilter);

            dataSource.ForEach(x =>
            {
                x.StartTime = DateConverter.ToLocal(x.StartTime, timezone);
                x.EndTime = DateConverter.ToLocal(x.EndTime, timezone);

                if (x.AircraftSchedulerDetailsVM.IsCheckOut)
                {
                    if (currentView == SchedulerView.Day || currentView == SchedulerView.Week || currentView == SchedulerView.Month)
                    {
                        x.CssClass = "checkedouthorizontally";
                    }
                    else
                    {
                        x.CssClass = "checkedout";
                    }
                }
                else if (x.AircraftSchedulerDetailsVM.CheckInTime != null)
                {
                    if (currentView == SchedulerView.Day || currentView == SchedulerView.Week || currentView == SchedulerView.Month)
                    {
                        x.CssClass = "checkedinhorizontally";
                    }
                    else
                    {
                        x.CssClass = "checkedin";
                    }
                }
                else
                {
                    if (currentView == SchedulerView.Day || currentView == SchedulerView.Week || currentView == SchedulerView.Month)
                    {
                        x.CssClass = "scheduledhorizontally";
                    }
                    else
                    {
                        x.CssClass = "scheduled";
                    }
                }
            });

            if (schedulerFilter.StartTime != null)
            {
                schedulerFilter.StartTime = DateConverter.ToLocal(schedulerFilter.StartTime, timezone);
            }

            if (schedulerFilter.EndTime != null)
            {
                schedulerFilter.EndTime = DateConverter.ToLocal(schedulerFilter.EndTime, timezone);
            }

            ChangeLoaderVisibilityAction(false);
            base.StateHasChanged();
        }

        private async Task<List<ResourceData>> GetAircraftData(UserPreferenceVM aircraftPreference)
        {
            allAircraftList = await AircraftService.ListAllAsync(dependecyParams,0);

            List<DE.Aircraft> aircraftList = new List<DE.Aircraft>();

            if (aircraftPreference != null)
            {
                List<long> aircraftIds = aircraftPreference.ListPreferencesIds.Select(long.Parse).ToList();
                aircraftList = allAircraftList.Where(p => aircraftIds.Contains(p.Id)).ToList();
            }
            else
            {
                aircraftList = allAircraftList;
            }

            multipleAircrafts = aircraftList.Select(p => p.Id).ToList();

            List<ResourceData> aircraftResourceList = new List<ResourceData>();

            foreach (DE.Aircraft aircraft in aircraftList)
            {
                aircraftResourceList.Add(new ResourceData { AircraftTailNo = aircraft.TailNo, Id = aircraft.Id });
            }

            return aircraftResourceList;
        }

        public void InitializeValues()
        {
            uiOptions.isDisplayRecurring = true;
            uiOptions.isDisplayMember1Dropdown = true;
            uiOptions.isDisplayAircraftDropDown = true;
            uiOptions.isDisplayFlightRoutes = false;
            uiOptions.isDisplayInstructor = false;
            uiOptions.isDisplayFlightInfo = false;
            uiOptions.isDisplayStandBy = true;
            uiOptions.isDisplayForm = true;
            uiOptions.isDisplayCheckOutOption = false;
            uiOptions.isDisplayMainForm = true;

            if (schedulerVM != null)
            {
                schedulerVM.IsDisplayMember2Dropdown = false;
            }
        }

        private async Task OnDoubleClickHandler(SchedulerItemDoubleClickEventArgs args)
        {
            SchedulerVM currentItem = args.Item as SchedulerVM;
            args.ShouldRender = false;

            await OpenAppointmentDialog(currentItem);
        }

        private async Task OnClickHandlerAsync(SchedulerItemClickEventArgs args)
        {
            SchedulerVM currentItem = args.Item as SchedulerVM;
            args.ShouldRender = false;

            await OpenAppointmentDialog(currentItem);
        }

        async Task EditHandler(SchedulerEditEventArgs args)
        {
            args.IsCancelled = true;
            await OpenCreateScheduleDialogAsync(args.Start, args.End);
        }

        private async Task OpenCreateScheduleDialogAsync(DateTime? startTime = null, DateTime? endTime = null, long? aircraftId = null)
        {

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Create, "Scheduler"))
            {
                //await DialogService.OpenAsync<UnAuthorized>("UnAuthorized",
                //  new Dictionary<string, object>() { { "UnAuthorizedMessage", "You are not authorized to create new reservation. Please contact to your administartor" } },
                //  new DialogOptions() { Width = "410px", Height = "165px" });

                return;
            }

            InitializeValues();

            isBusyAddButton = true;

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(dependecyParams, 0);

            if (startTime != null)
            {
                schedulerVM.StartTime = startTime.Value;
                schedulerVM.EndTime = endTime.Value;
            }
            else
            {
                schedulerVM.StartTime = DateTime.Now;
                schedulerVM.EndTime = DateTime.Now.AddMinutes(30);
            }

            if (aircraftId != null)
            {
                schedulerVM.AircraftId = aircraftId;
            }

            isBusyAddButton = false;
            isDisplayPopup = true;
            popupTitle = "Schedule Appointment";
        }

        public async Task OpenAppointmentDialog(SchedulerVM args)
        {
             ChangeLoaderVisibilityAction(true);

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(dependecyParams, args.Id);

            schedulerVM.StartTime = DateConverter.ToLocal(schedulerVM.StartTime, timezone);
            schedulerVM.EndTime = DateConverter.ToLocal(schedulerVM.EndTime, timezone);

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime = DateConverter.ToLocal(schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime.Value, timezone);
            }

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.CheckInTime = DateConverter.ToLocal(schedulerVM.AircraftSchedulerDetailsVM.CheckInTime.Value, timezone);
            }

            uiOptions.isDisplayForm = false;
            uiOptions.isDisplayCheckOutOption = false;

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime == null)
            {
                uiOptions.isDisplayCheckOutOption = true;
            }

            uiOptions.isDisplayMainForm = true;
            uiOptions.isDisplayCheckInButton = schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut;

             ChangeLoaderVisibilityAction(false);
            isDisplayPopup = true;
            popupTitle = "Schedule Appointment";
        }

        public async Task RefreshSchedulerDataSourceAsync(ScheduleOperations scheduleOperations)
        {
            if (scheduleOperations == ScheduleOperations.Schedule)
            {
                await LoadDataAsync();
                return;
            }

            if (scheduleOperations == ScheduleOperations.CheckOut)
            {
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = true;
                dataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.AircraftSchedulerDetailsVM.IsCheckOut = true; });
            }
            else if (scheduleOperations == ScheduleOperations.CheckIn)
            {
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = false;
                dataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.AircraftSchedulerDetailsVM.IsCheckOut = false; p.AircraftSchedulerDetailsVM.CheckInTime = DateTime.Now; });
            }
            else if (scheduleOperations == ScheduleOperations.UnCheckOut)
            {
                schedulerVM.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM();
                dataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM(); });
            }
            else
            {
                dataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.EndTime = schedulerVM.EndTime; });
            }

            await LoadDataAsync();
        }

        void OnAircraftsListChange()
        {
            if (multipleAircrafts == null)
            {
                aircraftsResourceList = new List<ResourceData>();
            }
            else
            {
                var aircraftList = allAircraftList.Where(p => multipleAircrafts.Contains(p.Id)).ToList();

                List<ResourceData> aircraftResourceList = new List<ResourceData>();

                foreach (DE.Aircraft aircraft in aircraftList)
                {
                    aircraftResourceList.Add(new ResourceData { AircraftTailNo = aircraft.TailNo, Id = aircraft.Id });
                }

                aircraftsResourceList = aircraftResourceList;
            }

            base.StateHasChanged();
        }

        async void GetAircraftsList(int value)
        {
            ChangeLoaderVisibilityAction(true);
            schedulerFilter.CompanyId = value;

            aircraftsResourceList = new List<ResourceData>();

            multipleAircrafts = new List<long>();
            allAircraftList = await AircraftService.ListAllAsync(dependecyParams, value);

            //scheduleRef.Rebind();
            await LoadDataAsync();
            ChangeLoaderVisibilityAction(false);
        }

        private void CloseDialog()
        {
            isDisplayPopup = false;
        }

        private void OpenDialog()
        {
            isDisplayPopup = true;
        }
    }

    public class ResourceData
    {
        public long Id { get; set; }

        public string AircraftTailNo { get; set; }
    }
    public class UIOptions
    {
        public bool isDisplayRecurring { get; set; }
        public bool isDisplayCheckInButton { get; set; }
        public bool isDisplayMainForm { get; set; }
        public bool isDisplayEditEndTimeForm { get; set; }
        public bool isBusyUnCheckOutButton { get; set; }
        public bool isBusyCheckOutButton { get; set; }
        public bool isVisibleDeleteDialog { get; set; }
        public bool isBusyDeleteButton { get; set; }
        public bool isDisplayCheckOutOption { get; set; }
        public bool isDisplayForm { get; set; }
        public bool isDisplayFlightInfo { get; set; }
        public bool isDisplayInstructor { get; set; }
        public bool isDisplayFlightRoutes { get; set; }
        public bool isDisplayAircraftDropDown { get; set; }
        public bool isDisplayStandBy { get; set; }
        public bool isDisplayMember1Dropdown { get; set; }

    }
}
