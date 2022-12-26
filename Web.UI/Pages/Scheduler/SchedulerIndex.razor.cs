﻿using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using DataModels.VM.UserPreference;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Utilities;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Web.UI.Data.AircraftSchedule;
using Web.UI.Models.Shared;
using Microsoft.AspNetCore.Components.Web;
using Web.UI.Models.Scheduler;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.UI.Pages.Scheduler
{
    partial class SchedulerIndex
    {
        #region params
        public TelerikScheduler<SchedulerVM> scheduleRef { get; set; }

        SchedulerVM schedulerVM;
        List<SchedulerVM> dataSource;
        public SchedulerView currentView { get; set; } = SchedulerView.Timeline;
        public List<string> resources = new List<string>() { "AircraftId" };
        public List<string> pilotResources = new List<string>() { "Member1Id" };
        List<ResourceData> aircraftsResourceList = new List<ResourceData>();
        List<ResourceData> pilotsResourceList = new List<ResourceData>();
        string moduleName = "Scheduler";
        public UIOptions uiOptions = new UIOptions();
        SchedulerFilter schedulerFilter = new SchedulerFilter();

        List<long> selectedAircraftList = new List<long>();
        List<long> selectedPilotsList = new List<long>();
        List<DropDownLargeValues> multiSelectAircraftsList = new List<DropDownLargeValues>();
        List<DropDownLargeValues> multiSelectPilotsList = new List<DropDownLargeValues>();

        DependecyParams dependecyParams;
        public List<ContextMenuItem> menuItems { get; set; }
        TelerikContextMenu<ContextMenuItem> contextMenu { get; set; }

        Create createScheduleRef;
        public bool isOpenFromContextMenu { get; set; }
        public bool displayAircraftScheduler { get; set; } = true;
        public bool isDisplayActiveTodayPilots { get; set; } = false;
        public int schedulerViewOption = 0;

        int multiDayDaysCount { get; set; } = 10;
        DateTime currentDate = DateTime.Now;

        public DateTime dayStart { get; set; } = new DateTime(2000, 1, 1, 0, 0, 0);
        public DateTime dayEnd { get; set; } = new DateTime(2000, 1, 1, 23, 0, 0);
        public DateTime workDayStart { get; set; } = new DateTime(2000, 1, 1, 9, 0, 0);
        public DateTime workDayEnd { get; set; } = new DateTime(2000, 1, 1, 17, 0, 0);

        List<RadioButtonItem> viewOptions { get; set; } = new List<RadioButtonItem>
        {
            new RadioButtonItem { Id = 0,Text = "Aircrafts" },
            new RadioButtonItem { Id = 1, Text = "Pilots" },
        };

        #endregion

        protected override async Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            dataSource = new List<SchedulerVM>();
            currentDate = DateConverter.ToLocal(DateTime.UtcNow, globalMembers.Timezone);

            InitializeValues();

            schedulerVM = new SchedulerVM();
            aircraftsResourceList = new List<ResourceData>();
            pilotsResourceList = new List<ResourceData>();
        }

        private async Task OpenDetailPopup()
        {
            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("ScheduleId", out link);

            if (link.Count() == 0 || link[0] == "")
            {
                return;
            }

            var base64EncodedBytes = Convert.FromBase64String(link[0]);
            string scheduleId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace("FlyManager", "");

            ChangeLoaderVisibilityAction(true);

            SchedulerVM schedulerVM = await AircraftSchedulerService.GetDetailsAsync(dependecyParams, Convert.ToInt64(scheduleId));

            ChangeLoaderVisibilityAction(false);

            await OpenAppointmentDialog(schedulerVM); 

            StateHasChanged();
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
                    pilotsResourceList = await GetPilotsList(0);
                }
                else
                {
                    schedulerFilter.Companies = await CompanyService.ListDropDownValues(dependecyParams);
                }

                await LoadDataAsync();
                ChangeLoaderVisibilityAction(false);

                await OpenDetailPopup();
            }
        }

        async Task ShowContextMenu(MouseEventArgs e, SchedulerVM clickedItem)
        {
            menuItems = new List<ContextMenuItem>();

            if (clickedItem.AircraftSchedulerDetailsVM.IsCheckOut)
            {
                menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Check In", Type = ScheduleOperations.CheckIn });
                menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Edit End Time", Type = ScheduleOperations.UpdateEndTime });
                menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Un-Check Out", Type = ScheduleOperations.UnCheckOut });
            }
            else
            {
                if (clickedItem.AircraftSchedulerDetailsVM.CheckInTime != null)
                {
                    menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Edit End Time", Type = ScheduleOperations.UpdateEndTime });
                    menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Edit Flight Time", Type = ScheduleOperations.UpdateFlightTime });
                }
                else
                {
                    menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Check out", Type = ScheduleOperations.CheckOut });
                    menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Edit", Type = ScheduleOperations.Edit });
                    menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "Delete", Type = ScheduleOperations.Delete });
                }
            }

            menuItems.Add(new ContextMenuItem() { ScheduleDetails = clickedItem, Text = "View Details", Type = ScheduleOperations.ViewDetails });

            contextMenu.Data = menuItems;
            await contextMenu.ShowAsync(e.ClientX, e.ClientY);
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

            schedulerFilter.StartTime = DateConverter.ToUTC(schedulerFilter.StartTime.Date, globalMembers.Timezone);
            schedulerFilter.EndTime = DateConverter.ToUTC(schedulerFilter.EndTime.Date.AddDays(1).AddTicks(-1), globalMembers.Timezone);

            dataSource = await AircraftSchedulerService.ListAsync(dependecyParams, schedulerFilter);

            dataSource.ForEach(x =>
            {
                x.StartTime = DateConverter.ToLocal(x.StartTime, globalMembers.Timezone);
                x.EndTime = DateConverter.ToLocal(x.EndTime, globalMembers.Timezone);

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
                schedulerFilter.StartTime = DateConverter.ToLocal(schedulerFilter.StartTime, globalMembers.Timezone);
            }

            if (schedulerFilter.EndTime != null)
            {
                schedulerFilter.EndTime = DateConverter.ToLocal(schedulerFilter.EndTime, globalMembers.Timezone);
            }

            ChangeLoaderVisibilityAction(false);
            base.StateHasChanged();
        }

        private async Task<List<ResourceData>> GetPilotsList(int companyId)
        {
            multiSelectPilotsList = await UserService.ListDropDownValuesByCompanyId(dependecyParams,companyId);
            pilotsResourceList = new List<ResourceData>();

            selectedPilotsList = multiSelectPilotsList.Select(p => p.Id).ToList();
            foreach (DropDownLargeValues pilot in multiSelectPilotsList)
            {
                pilotsResourceList.Add(new ResourceData { Text = pilot.Name, Id = pilot.Id });
            }

            return pilotsResourceList;
        }

        private async Task GetAircraftData(int value = 0)
        {
            multiSelectAircraftsList = await AircraftService.ListDropdownValuesByCompanyId(dependecyParams, value);
        }

        private async Task<List<ResourceData>> GetAircraftData(UserPreferenceVM aircraftPreference)
        {
            await GetAircraftData(0);
            List<DropDownLargeValues> aircraftList = new List<DropDownLargeValues>();

            if (aircraftPreference != null)
            {
                List<long> aircraftIds = aircraftPreference.ListPreferencesIds.Select(long.Parse).ToList();
                aircraftList = multiSelectAircraftsList.Where(p => aircraftIds.Contains(p.Id)).ToList();
            }
            else
            {
                aircraftList = multiSelectAircraftsList;
            }

            selectedAircraftList = aircraftList.Select(p => p.Id).ToList();

            List<ResourceData> aircraftResourceList = new List<ResourceData>();

            foreach (DropDownLargeValues aircraft in aircraftList)
            {
                aircraftResourceList.Add(new ResourceData { Text = aircraft.Name, Id = aircraft.Id });
            }

            return aircraftResourceList;
        }

        public void InitializeValues()
        {
            uiOptions.IsDisplayRecurring = true;
            uiOptions.IsDisplayMember1Dropdown = true;
            uiOptions.IsDisplayAircraftDropDown = true;
            uiOptions.IsDisplayFlightRoutes = false;
            uiOptions.IsDisplayInstructor = false;
            uiOptions.IsDisplayFlightInfo = false;
            uiOptions.IsDisplayStandBy = true;
            uiOptions.IsDisplayForm = true;
            uiOptions.IsDisplayCheckOutOption = false;
            uiOptions.IsDisplayMainForm = true;

            if (schedulerVM != null)
            {
                schedulerVM.IsDisplayMember2Dropdown = false;
            }
        }

        private async Task OnDoubleClickHandler(SchedulerItemDoubleClickEventArgs args)
        {
            SchedulerVM currentItem = args.Item as SchedulerVM;
            args.ShouldRender = false;
            isOpenFromContextMenu = false;

            operationType = OperationType.Create;

            await OpenAppointmentDialog(currentItem);
        }

        private async Task OnClickHandlerAsync(SchedulerItemClickEventArgs args)
        {
            SchedulerVM currentItem = args.Item as SchedulerVM;
            args.ShouldRender = false;
            isOpenFromContextMenu = false;
            operationType = OperationType.Create;

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

            await SetSchedulerDetails(args.Id);

            uiOptions.IsDisplayForm = false;
            uiOptions.IsDisplayCheckOutOption = false;

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime == null)
            {
                uiOptions.IsDisplayCheckOutOption = true;
            }

            uiOptions.IsDisplayMainForm = true;
            uiOptions.IsDisplayCheckInButton = schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut;

            ChangeLoaderVisibilityAction(false);
            isDisplayPopup = true;
            popupTitle = "Schedule Appointment";
        }

        private async Task SetSchedulerDetails(long id)
        {
            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(dependecyParams, id);

            schedulerVM.StartTime = DateConverter.ToLocal(schedulerVM.StartTime, globalMembers.Timezone);
            schedulerVM.EndTime = DateConverter.ToLocal(schedulerVM.EndTime, globalMembers.Timezone);

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime = DateConverter.ToLocal(schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime.Value, globalMembers.Timezone);
            }

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.CheckInTime = DateConverter.ToLocal(schedulerVM.AircraftSchedulerDetailsVM.CheckInTime.Value, globalMembers.Timezone);
            }
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

        void UpdateSelectedAircraftData(List<long> selectedData)
        {
            selectedAircraftList = selectedData;
            base.StateHasChanged();
        }

        void OnAircraftsListChange(List<long> selectedData)
        {
            selectedAircraftList = selectedData;
            if (selectedAircraftList == null || selectedAircraftList.Count() == 0)
            {
                resources = new List<string>();
                aircraftsResourceList = new List<ResourceData>();
            }
            else
            {
                resources = new List<string>() { "AircraftId" };
                List<DropDownLargeValues> aircraftList = new List<DropDownLargeValues>();

                aircraftList = multiSelectAircraftsList.Where(p => selectedAircraftList.Contains(p.Id)).ToList();

                List<ResourceData> aircraftResourceList = new List<ResourceData>();

                foreach (DropDownLargeValues aircraft in aircraftList)
                {
                    aircraftResourceList.Add(new ResourceData { Text = aircraft.Name, Id = aircraft.Id });
                }

                aircraftsResourceList = aircraftResourceList;
            }

            base.StateHasChanged();
        }

        void UpdateSelectedPilotData(List<long> selectedData)
        {
            selectedPilotsList = selectedData;
            base.StateHasChanged();
        }

        public async Task GetAutocompleteData(int companyId)
        {
            schedulerFilter.CompanyId = companyId;
            await GetAircraftData(companyId);
            await GetPilotsList(companyId);
        }

        void OnPilotsListChange(List<long> selectedData)
        {
            selectedPilotsList = selectedData;
            if (selectedPilotsList == null || selectedPilotsList.Count() == 0)
            {
                resources = new List<string>();
                pilotsResourceList = new List<ResourceData>();
            }
            else
            {
                resources = new List<string>() { "Member1Id" };
                List<DropDownLargeValues> aircraftList = new List<DropDownLargeValues>();

                aircraftList = multiSelectPilotsList.Where(p => selectedPilotsList.Contains(p.Id)).ToList();

                List<ResourceData> pilotResourceList = new List<ResourceData>();

                foreach (DropDownLargeValues aircraft in aircraftList)
                {
                    pilotResourceList.Add(new ResourceData { Text = aircraft.Name, Id = aircraft.Id });
                }

                pilotsResourceList = pilotResourceList;
            }

            base.StateHasChanged();
        }

        async void GetAircraftsList(int value)
        {
            ChangeLoaderVisibilityAction(true);
            schedulerFilter.CompanyId = value;

            resources = new List<string>();
            aircraftsResourceList = new List<ResourceData>();

            selectedAircraftList = new List<long>();
            await GetAircraftData(value);

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

        async Task OnContextMenuClickItem(ContextMenuItem clickedItem)
        {
            operationType = OperationType.Create;
            isOpenFromContextMenu = true;
            schedulerVM = dataSource.Where(p => p.Id == clickedItem.ScheduleDetails.Id).FirstOrDefault();
            popupTitle = "Schedule Appointment";

            ChangeLoaderVisibilityAction(true);
            await contextMenu.HideAsync();

            if (clickedItem.Type == ScheduleOperations.ViewDetails)
            {
                await OpenAppointmentDialog(clickedItem.ScheduleDetails);
            }
            else if (clickedItem.Type == ScheduleOperations.Delete)
            {
                operationType = OperationType.Delete;
                isDisplayPopup = true;
            }
            else if (clickedItem.Type == ScheduleOperations.UnCheckOut)
            {
                operationType = OperationType.UnCheckOut;
                isDisplayPopup = true;
            }
            else if (clickedItem.Type == ScheduleOperations.Edit)
            {
                await SetSchedulerDetails(schedulerVM.Id);
                isDisplayPopup = true;
                base.StateHasChanged();
                createScheduleRef.OpenForm();
            }
            else if (clickedItem.Type == ScheduleOperations.CheckIn)
            {
                await SetSchedulerDetails(schedulerVM.Id);
                isDisplayPopup = true;
                base.StateHasChanged();
                createScheduleRef.CheckInAircraft();
            }
            else if (clickedItem.Type == ScheduleOperations.UpdateFlightTime)
            {
                await SetSchedulerDetails(schedulerVM.Id);
                isDisplayPopup = true;
                base.StateHasChanged();
                createScheduleRef.EditFlightTime();
            }
            else if (clickedItem.Type == ScheduleOperations.UpdateEndTime)
            {
                await SetSchedulerDetails(schedulerVM.Id);
                isDisplayPopup = true;
                base.StateHasChanged();
                createScheduleRef.ShowEditEndTimeForm();
            }

            else if (clickedItem.Type == ScheduleOperations.CheckOut)
            {
                await CheckOutAircraft();
            }

            ChangeLoaderVisibilityAction(false);
        }

        public async Task DeleteAsync()
        {
            uiOptions.IsBusyDeleteButton = true;

            CurrentResponse response = await AircraftSchedulerService.DeleteAsync(dependecyParams, schedulerVM.Id);

            uiOptions.IsBusyDeleteButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog();
            }

            await LoadDataAsync();
        }

        private async Task UnCheckOutAppointment()
        {
            uiOptions.IsBusyUnCheckOutButton = true;

            CurrentResponse response = await AircraftSchedulerDetailService.UnCheckOut(dependecyParams, schedulerVM.AircraftSchedulerDetailsVM.Id);

            uiOptions.IsBusyUnCheckOutButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM();
                base.StateHasChanged();
                await RefreshSchedulerDataSourceAsync(ScheduleOperations.UnCheckOut);
                CloseDialog();
            }
        }

        public async Task CheckOutAircraft()
        {
            CurrentResponse response = await AircraftSchedulerDetailService.IsAircraftAlreadyCheckOutAsync(dependecyParams, schedulerVM.AircraftId.GetValueOrDefault());

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if ((bool)response.Data == true)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
                }
                else
                {
                    await CheckOut();
                }
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }

        }

        public async void DisplayActivePilot(object value)
        {
            isDisplayActiveTodayPilots = (bool)value;

            if (isDisplayActiveTodayPilots)
            {
                pilotsResourceList = pilotsResourceList.Where(p=> dataSource.Select(p=>p.Member1Id).Contains(p.Id)).ToList();
            }
            else
            {
                await GetPilotsList(globalMembers.CompanyId);
            }

            scheduleRef.Rebind();
        }

        void OnschedulerViewOptionValueChange()
        {
            if (schedulerViewOption == 0)
            {
                displayAircraftScheduler = true;
            }
            else
            {
                displayAircraftScheduler = false;
            }
        }

        private async Task CheckOut()
        {
            CurrentResponse response = await AircraftSchedulerDetailService.CheckOut(dependecyParams, schedulerVM.Id);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = true;
                await RefreshSchedulerDataSourceAsync(ScheduleOperations.CheckOut);
                CloseDialog();
            }
        }
    }
}
