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
using Newtonsoft.Json;

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

        public bool isDisplayRecurring, isDisplayMember1Dropdown, isDisplayMember2Dropdown, isDisplayStandBy,
            isDisplayAircraftDropDown, isDisplayFlightRoutes, isDisplayInstructor, isDisplayFlightInfo, dialogVisibility,
            isDisplayForm, isDisplayCheckoutOption, isBusyDeleteButton, isVisibleDeleteDialog, isBusyCheckOutButton;

        public bool isBusy;
        DateTime currentDate = DateTime.Now;

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
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
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
                isDisplayMember2Dropdown = true;
                isDisplayFlightRoutes = true;
            }

            else if (args.Value == (int)ScheduleActivityType.RenterFlight)
            {
                isDisplayMember2Dropdown = true;
                isDisplayFlightRoutes = true;
                isDisplayFlightInfo = true;
            }

            else if (args.Value == (int)ScheduleActivityType.TourFlight)
            {
                isDisplayMember2Dropdown = true;
                isDisplayFlightRoutes = true;
                isDisplayFlightInfo = true;
            }

            else if (args.Value == (int)ScheduleActivityType.StudentSolo)
            {
                isDisplayFlightRoutes = true;
                isDisplayFlightInfo = true;
            }

            else if (args.Value == (int)ScheduleActivityType.Maintenance)
            {
                isDisplayRecurring = false;
                isDisplayMember1Dropdown = false;
                isDisplayStandBy = false;
            }

            else if (args.Value == (int)ScheduleActivityType.DiscoveryFlight)
            {
                isDisplayInstructor = true;
            }

            else if (args.Value == (int)ScheduleActivityType.DualFlightTraining)
            {
                isDisplayInstructor = true;
            }

            else if (args.Value == (int)ScheduleActivityType.GroundTraining)
            {
                //IsDisplayAircraftDropDown = false;
                isDisplayInstructor = true;
            }

            base.StateHasChanged();
        }

        private async Task CheckOutAircraft()
        {
            await SetCheckOutButtonState(true);

            // check if someone else already checked out it 
            CurrentResponse response = await AircraftSchedulerService.IsAircraftAlreadyCheckOutAsync(_httpClient, schedulerVM.AircraftId.GetValueOrDefault());

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                if (response.Data == "false")
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                    NotificationService.Notify(message);
                }
                else
                {
                    // Enter details in details table
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }

            await SetCheckOutButtonState(false);
        }

        private void CloseDialog()
        {
            dialogVisibility = false;
            base.StateHasChanged();
        }

        public void InitializeValues()
        {
            isDisplayRecurring = true;
            isDisplayMember1Dropdown = true;
            isDisplayAircraftDropDown = true;
            isDisplayMember2Dropdown = false;
            isDisplayFlightRoutes = false;
            isDisplayInstructor = false;
            isDisplayFlightInfo = false;
            isDisplayStandBy = true;
            isDisplayForm = true;
            isDisplayCheckoutOption = false;
        }

        public async Task OpenCreateAppointmentDialog(CellClickEventArgs args)
        {
            InitializeValues();

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, 0);

            schedulerVM.StartTime = args.StartTime;
            schedulerVM.EndTime = args.EndTime;

            var SelectedResource = ScheduleRef.GetResourceByIndex(args.GroupIndex);
            var groupData = JsonConvert.DeserializeObject<SchedulerVM>(JsonConvert.SerializeObject(SelectedResource.GroupData));
            schedulerVM.AircraftId = groupData.AircraftId;

            args.Cancel = true;
            dialogVisibility = true;
        }

        public async Task OnEventClick(EventClickArgs<SchedulerVM> args)
        {
            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, args.Event.Id);
            args.Cancel = true;
            dialogVisibility = true;

            isDisplayForm = false;
            isDisplayCheckoutOption = true;
        }

        private void OpenForm()
        {
            isDisplayForm = true;
            base.StateHasChanged();
        }

        private async void OnValidSubmit()
        {
            isDisplayCheckoutOption = false;

            if (isDisplayForm)
            {
                isDisplayForm = false;
                base.StateHasChanged();

                return;
            }

            isBusy = true;

            CurrentResponse response = await AircraftSchedulerService.SaveandUpdateAsync(_httpClient, schedulerVM);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                dialogVisibility = false;
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

        async Task DeleteAsync()
        {
            await SetDeleteButtonState(true);

            CurrentResponse response = await AircraftSchedulerService.DeleteAsync(_httpClient, schedulerVM.Id);

            await SetDeleteButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                DialogService.Close();
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }

           await ScheduleRef.DeleteEventAsync(schedulerVM.Id, CurrentAction.Delete);
        }

        private void CloseDeleteDialog()
        {
            //IsVisibleDeleteDialog = false; 
            dialogVisibility = true;
        }

        private async Task SetDeleteButtonState(bool isBusy)
        {
            isBusyDeleteButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        private async Task SetCheckOutButtonState(bool isBusy)
        {
            isBusyCheckOutButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
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
