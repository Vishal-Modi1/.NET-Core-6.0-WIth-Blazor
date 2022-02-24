using DataModels.VM.Scheduler;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.Blazor.Schedule;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DE = DataModels.Entities;
using DataModels.VM.Common;
using Radzen;
using FSM.Blazor.Extensions;
using Newtonsoft.Json;
using DataModels.VM.AircraftEquipment;
using DataModels.Entities;
using DataModels.Enums;
using Utilities;
using DataModels.Constants;

namespace FSM.Blazor.Pages.Scheduler
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        public SfSchedule<SchedulerVM> ScheduleRef;

        SchedulerVM schedulerVM;
        DE.AircraftEquipmentTime aircraftEquipmentTime = new DE.AircraftEquipmentTime();

        List<SchedulerVM> DataSource;

        public View CurrentView { get; set; } = View.TimelineDay;

        public string[] Resources { get; set; } = { "Aircrafts" };
        public ObservableCollection<ResourceData> ObservableAircraftsData { get; set; }

        string moduleName = "Scheduler";

        public UIOptions uiOptions = new UIOptions();

        string timezone = "";
        DateTime currentDate = DateTime.Now;

        SchedulerFilter schedulerFilter = new SchedulerFilter();

        private bool isDisplayLoader { get; set; } = false;
        private bool isDisplayScheduler { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            timezone = ClaimManager.GetClaimValue(authenticationStateProvider, CustomClaimTypes.TimeZone);

            DateTime currentDate = DateConverter.ToLocal(DateTime.UtcNow, timezone);
            isDisplayLoader = true;
            InitializeValues();

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            schedulerVM = new SchedulerVM();
            schedulerVM.ScheduleActivitiesList = new List<DropDownValues>();

            ObservableAircraftsData = new ObservableCollection<ResourceData>(await GetAircraftData());
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                isDisplayScheduler = false;

                await LoadDataAsync();

                isDisplayScheduler = true;

                isDisplayLoader = false;
                base.StateHasChanged();
            }
        }

        public async Task OnActionCompletedAsync(ActionEventArgs<SchedulerVM> args)
        {
            if (args.ActionType == ActionType.ViewNavigate || args.ActionType == ActionType.DateNavigate)
            {
                await LoadDataAsync();
            }
        }

        public async Task LoadDataAsync()
        {
            isDisplayLoader = true;
            base.StateHasChanged();

            if (ScheduleRef != null)
            {
                List<DateTime> viewDates = ScheduleRef.GetCurrentViewDates();

                schedulerFilter.StartTime = viewDates.First();
                schedulerFilter.EndTime = viewDates.Last();
            }
            else
            {
                schedulerFilter.StartTime = DateTime.UtcNow.Date;
                schedulerFilter.EndTime = DateTime.UtcNow.Date;
            }

            if (schedulerFilter.StartTime != null)
            {
                schedulerFilter.StartTime = DateConverter.ToUTC(schedulerFilter.StartTime.Date, timezone);
            }

            if (schedulerFilter.EndTime != null)
            {
                schedulerFilter.EndTime = DateConverter.ToUTC(schedulerFilter.EndTime.Date.AddDays(1).AddTicks(-1), timezone);
            }

            DataSource = await AircraftSchedulerService.ListAsync(_httpClient, schedulerFilter);

            DataSource.ForEach(x =>
            {
                x.StartTime = DateConverter.ToLocal(x.StartTime, timezone);
                x.EndTime = DateConverter.ToLocal(x.EndTime, timezone);

                if (x.AircraftSchedulerDetailsVM.IsCheckOut)
                {
                    if (CurrentView == View.Day || CurrentView == View.Week || CurrentView == View.Month)
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
                    if (CurrentView == View.Day || CurrentView == View.Week || CurrentView == View.Month)
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
                    if (CurrentView == View.Day || CurrentView == View.Week || CurrentView == View.Month)
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

            isDisplayLoader = false;

            base.StateHasChanged();
        }

        private async Task<List<ResourceData>> GetAircraftData()
        {
            List<DE.Aircraft> aircraftList = await AircraftService.ListAllAsync(_httpClient);

            List<ResourceData> aircraftResourceList = new List<ResourceData>();

            foreach (DE.Aircraft aircraft in aircraftList)
            {
                aircraftResourceList.Add(new ResourceData { AircraftTailNo = aircraft.TailNo, Id = aircraft.Id });
            }

            return aircraftResourceList;
        }

        public void OnEventRendered(EventRenderedArgs<SchedulerVM> args)
        {
            if (args.Data.AircraftSchedulerDetailsVM.IsCheckOut)
            {
                args.CssClasses = new List<string>() { "checkedout", "checkedouthorizontally" };
            }
            else
            {
                if (args.Data.AircraftSchedulerDetailsVM.CheckInTime != null)
                {
                    args.CssClasses = new List<string>() { "checkedin", "checkedinhorizontally" };
                }
                else
                {
                    args.CssClasses = new List<string>() { "scheduled", "scheduledhorizontally" };
                }
            }
        }

        public void InitializeValues()
        {
            uiOptions.isDisplayRecurring = true;
            uiOptions.isDisplayMember1Dropdown = true;
            uiOptions.isDisplayAircraftDropDown = true;
            uiOptions.isDisplayMember2Dropdown = false;
            uiOptions.isDisplayFlightRoutes = false;
            uiOptions.isDisplayInstructor = false;
            uiOptions.isDisplayFlightInfo = false;
            uiOptions.isDisplayStandBy = true;
            uiOptions.isDisplayForm = true;
            uiOptions.isDisplayCheckOutOption = false;
            uiOptions.isDisplayMainForm = true;
        }

        public async Task OpenCreateAppointmentDialog(CellClickEventArgs args)
        {
            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Create, "Scheduler"))
            {
                await DialogService.OpenAsync<UnAuthorized>("Un Authorized",
                  new Dictionary<string, object>() { { "UnAuthorizedMessage", "You are not authorized to create new reservation. Please contact to your administartor" } },
                  new DialogOptions() { Width = "410px", Height = "165px" });

                return;
            }

            isDisplayLoader = true;

            InitializeValues();

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, 0);

            schedulerVM.StartTime = args.StartTime;
            schedulerVM.EndTime = args.EndTime;

            var SelectedResource = ScheduleRef.GetResourceByIndex(args.GroupIndex);
            var groupData = JsonConvert.DeserializeObject<SchedulerVM>(JsonConvert.SerializeObject(SelectedResource.GroupData));
            schedulerVM.AircraftId = groupData.AircraftId;

            args.Cancel = true;
            uiOptions.dialogVisibility = true;

            isDisplayLoader = false;
        }

        public async Task OnEventClick(EventClickArgs<SchedulerVM> args)
        {
            isDisplayLoader = true;

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, args.Event.Id);

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

            args.Cancel = true;
            uiOptions.dialogVisibility = true;

            uiOptions.isDisplayForm = false;
            uiOptions.isDisplayCheckOutOption = false;

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime == null)
            {
                uiOptions.isDisplayCheckOutOption = true;
            }

            uiOptions.isDisplayMainForm = true;
            uiOptions.isDisplayCheckInButton = schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut;

            isDisplayLoader = false;
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
                DataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.AircraftSchedulerDetailsVM.IsCheckOut = true; });
            }
            else if (scheduleOperations == ScheduleOperations.CheckIn)
            {
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = false;
                DataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.AircraftSchedulerDetailsVM.IsCheckOut = false; p.AircraftSchedulerDetailsVM.CheckInTime = DateTime.Now; });
            }
            else if (scheduleOperations == ScheduleOperations.UnCheckOut)
            {
                schedulerVM.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM();
                DataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM(); });
            }
            else
            {
                DataSource.Where(p => p.Id == schedulerVM.Id).ToList().ForEach(p => { p.EndTime = schedulerVM.EndTime; });
            }

            await ScheduleRef.RefreshEventsAsync();
            base.StateHasChanged();
        }

        private void CloseDialog()
        {
            uiOptions.dialogVisibility = false;
        }

        private void OpenDialog()
        {
            uiOptions.dialogVisibility = true;
            base.StateHasChanged();
        }

        public async Task DeleteEventAsync()
        {
            await ScheduleRef.DeleteEventAsync(schedulerVM.Id, CurrentAction.Delete);

            base.StateHasChanged();
        }

        public class ResourceData : INotifyPropertyChanged
        {
            public long Id { get; set; }

            private string aircraftTailNo { get; set; }

            public string AircraftTailNo
            {
                get { return aircraftTailNo; }
                set
                {
                    this.aircraftTailNo = value;
                    NotifyPropertyChanged("AircraftTailNo");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }
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
        public bool dialogVisibility { get; set; }
        public bool isDisplayFlightInfo { get; set; }
        public bool isDisplayInstructor { get; set; }
        public bool isDisplayFlightRoutes { get; set; }
        public bool isDisplayAircraftDropDown { get; set; }
        public bool isDisplayStandBy { get; set; }
        public bool isDisplayMember2Dropdown { get; set; }
        public bool isDisplayMember1Dropdown { get; set; }
    }
}
