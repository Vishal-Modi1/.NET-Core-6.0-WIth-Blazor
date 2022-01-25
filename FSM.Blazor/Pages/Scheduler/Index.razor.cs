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
using DataModels.Enums;
using Syncfusion.Blazor.DropDowns;
using Radzen;
using FSM.Blazor.Extensions;

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

        List<SchedulerVM> DataSource;

        public string[] Resources { get; set; } = { "Aircrafts" };
        public ObservableCollection<ResourceData> ObservableAircraftsData { get; set; }

        string moduleName = "Scheduler";

        public bool IsDisplayRecurring, IsDisplayMember1Dropdown, IsDisplayMember2Dropdown, IsDisplayStandBy,
            IsDisplayAircraftDropDown, IsDisplayFlightRoutes, IsDisplayInstructor, IsDisplayFlightInfo, DialogVisibility;

        public bool isBusy;
        DateTime CurrentDate = DateTime.Now;
        
        protected override async Task OnInitializedAsync()
        {
            InitializeValues();

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            schedulerVM = new SchedulerVM();
            schedulerVM.ScheduleActivitiesList = new List<DropDownValues>();

            ObservableAircraftsData = new ObservableCollection<ResourceData>(await GetAircraftData());

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            DataSource = await AircraftSchedulerService.ListAsync(_httpClient, new SchedulerFilter());

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

        public void OnActivityTypeValueChanged(ChangeEventArgs<int?, DropDownValues> args)
        {
            InitializeValues();

            if (args.Value == (int)ScheduleActivityType.CharterFlight)
            {
                IsDisplayMember2Dropdown = true;
                IsDisplayFlightRoutes = true;
            }

            else if (args.Value == (int)ScheduleActivityType.RenterFlight)
            {
                IsDisplayMember2Dropdown = true;
                IsDisplayFlightRoutes = true;
                IsDisplayFlightInfo = true;
            }

            else if (args.Value == (int)ScheduleActivityType.TourFlight)
            {
                IsDisplayMember2Dropdown = true;
                IsDisplayFlightRoutes = true;
                IsDisplayFlightInfo = true;
            }

            else if (args.Value == (int)ScheduleActivityType.StudentSolo)
            {
                IsDisplayFlightRoutes = true;
                IsDisplayFlightInfo = true;
            }

            else if (args.Value == (int)ScheduleActivityType.Maintenance)
            {
                IsDisplayRecurring = false;
                IsDisplayMember1Dropdown = false;
                IsDisplayStandBy = false;
            }

            else if (args.Value == (int)ScheduleActivityType.DiscoveryFlight)
            {
                IsDisplayInstructor = true;
            }

            else if (args.Value == (int)ScheduleActivityType.DualFlightTraining)
            {
                IsDisplayInstructor = true;
            }

            else if (args.Value == (int)ScheduleActivityType.GroundTraining)
            {
                IsDisplayAircraftDropDown = false;
                IsDisplayInstructor = true;
            }

            base.StateHasChanged();
        }

        public void InitializeValues()
        {
            IsDisplayRecurring = true;
            IsDisplayMember1Dropdown = true;
            IsDisplayAircraftDropDown = true;
            IsDisplayMember2Dropdown = false;
            IsDisplayFlightRoutes = false;
            IsDisplayInstructor = false;
            IsDisplayFlightInfo = false;
            IsDisplayStandBy = true;
        }

        public async Task OpenCreateAppointmentDialog(CellClickEventArgs args)
        {
            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, 0);

            schedulerVM.StartTime = args.StartTime;
            schedulerVM.EndTime = args.EndTime;

            args.Cancel = true;
            DialogVisibility = true;
        }

        public async Task OnEventClick(EventClickArgs<SchedulerVM> args)
        {
            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, args.Event.Id);

            args.Cancel = true;
            DialogVisibility = true;
        }

        private async void OnValidSubmit() //triggers on save button click
        {
            isBusy = true;
            base.StateHasChanged();

            CurrentResponse response = await AircraftSchedulerService.SaveandUpdateAsync(_httpClient, schedulerVM);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogVisibility = false ;
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }

            isBusy = false;

            await LoadDataAsync();
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
}
