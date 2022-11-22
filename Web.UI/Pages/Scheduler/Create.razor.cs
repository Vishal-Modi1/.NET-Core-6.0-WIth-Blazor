using DataModels.VM.Scheduler;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;
using DataModels.Entities;
using DataModels.Enums;
using Utilities;
using DataModels.Constants;
using Web.UI.Models.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Web.UI.Models.Scheduler;
using DataModels.VM.ExternalAPI.Airport;
using Telerik.Blazor.Components;
using Newtonsoft.Json;

namespace Web.UI.Pages.Scheduler
{
    partial class Create
    {
        #region Params

        [Parameter] public SchedulerVM schedulerVM { get; set; }
        [Parameter] public UIOptions uiOptions { get; set; }
        [Parameter] public EventCallback InitializeValuesParentEvent { get; set; }
        [Parameter] public EventCallback DeleteParentEvent { get; set; }
        [Parameter] public EventCallback<ScheduleOperations> RefreshSchedulerDataSourceParentEvent { get; set; }
        [Parameter] public bool IsOpenFromContextMenu { get; set; }
        [Parameter] public EventCallback CloseDialogParentEvent { get; set; }
        [Parameter] public EventCallback OpenDialogParentEvent { get; set; }
        [Parameter] public EventCallback LoadDataParentEvent { get; set; }

        #endregion

        DependecyParams dependecyParams;
        string timezone = "";

        public bool isAllowToEdit;
        public bool isAllowToDelete;
        AirportAPIFilter airportAPIFilter = new AirportAPIFilter();
        bool isValidAirportsSelected = false;
        AirportDetailsViewModel airportDetails = new ();

        List<DropDownGuidValues> departureAirportList, arrivalAirpotList;
        List<RadioButtonItem> flightTypes { get; set; } = new List<RadioButtonItem>
        {
            new RadioButtonItem { Id = 1, Text = "Local" },
            new RadioButtonItem { Id = 2, Text = "Cross Country" },
        };
        List<RadioButtonItem> flightRules { get; set; } = new List<RadioButtonItem>
        {
            new RadioButtonItem { Id = 1, Text = "VFR" },
            new RadioButtonItem { Id = 2, Text = "IFR" },
        };

        protected override async Task OnInitializedAsync()
        {
            timezone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            // user should be superadmin, admin or owner of reservation to update or delete it

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);

            bool isCreator = (userId == schedulerVM.CreatedBy || userId == schedulerVM.Member1Id);

            if (globalMembers.IsSuperAdmin || globalMembers.IsAdmin || isCreator)
            {
                isAllowToDelete = true;
                isAllowToEdit = true;
            }
        }

        private async void OnValidSubmit()
        {

            uiOptions.IsDisplayCheckOutOption = false;

            if (!isValidAirportsSelected)
            {
                isValidAirportsSelected = IsValidAirportsSelectedAsync();
            }

            if (!isValidAirportsSelected)
            {
                return;
            }

            if (uiOptions.IsDisplayForm)
            {
                ManageValues();
                uiOptions.IsDisplayForm = false;
                base.StateHasChanged();

                return;
            }

            isBusySubmitButton = true;
            ChangeLoaderVisibilityAction(true);

            schedulerVM.AircraftEquipmentsTimeList.Clear();
            CurrentResponse response = await AircraftSchedulerService.SaveandUpdateAsync(dependecyParams, schedulerVM, DateConverter.ToUTC(schedulerVM.StartTime, timezone), DateConverter.ToUTC(schedulerVM.EndTime, timezone));

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (response.Data == null)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
                }
                else
                {
                    CloseDialog();
                    globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, response.Message);
                }
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }

            isBusySubmitButton = false;
            ChangeLoaderVisibilityAction(false);
            CloseDialog();
            await LoadDataAsync();
        }

        private bool IsValidAirportsSelectedAsync()
        {
            bool isValid = true;
            isBusySubmitButton = true;

            if(departureAirportList == null)
            {
                return isValid;
            }

            var departureAirport = departureAirportList.Where(p => p.Name.ToLower() == schedulerVM.DepartureAirport.ToLower()).FirstOrDefault();

            if (departureAirport == null)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Departure airport is not valid");
                isValid = false;
            }
            else
            {
                schedulerVM.DepartureAirportId = departureAirport.Id;

                var arrivalAirport = arrivalAirpotList.Where(p => p.Name.ToLower() == schedulerVM.ArrivalAirport.ToLower()).FirstOrDefault();

                if (arrivalAirport == null)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Arrival airport is not valid");
                    isValid = false;
                }
                else
                {
                    schedulerVM.ArrivalAirportId = arrivalAirport.Id;
                }
            }

            isBusySubmitButton = false;
            return isValid;
        }

        public void OpenUnCheckOutDialog()
        {
            childPopupTitle = "Un-Check out appointment";
            isDisplayChildPopup = true;
            operationType = OperationType.UnCheckOut;
        }

        public void OpenDeleteDialog()
        {
            childPopupTitle = "Delete Appointment";
            isDisplayChildPopup = true;
            operationType = OperationType.Delete;

            base.StateHasChanged();
        }

        private void CloseDialog()
        {
            CloseDialogParentEvent.InvokeAsync();
        }

        private void OpenDialog()
        {
            OpenDialogParentEvent.InvokeAsync();
        }

        public void CheckInAircraft()
        {
            uiOptions.IsDisplayMainForm = false;
            uiOptions.IsDisplayCheckInForm = true;
            uiOptions.IsAllowToAddDiscrepancy = true;
        }

        public void ShowEditEndTimeForm()
        {
            uiOptions.IsDisplayMainForm = false;
            uiOptions.IsDisplayCheckInForm = false;
            uiOptions.IsDisplayEditEndTimeForm = true;
        }

        public void EditFlightTime()
        {
            uiOptions.IsDisplayMainForm = false;
            uiOptions.IsDisplayCheckInForm = true;

            foreach (AircraftEquipmentTimeVM aircraftEquipmentTimeVM in schedulerVM.AircraftEquipmentsTimeList)
            {
                AircraftScheduleHobbsTime aircraftScheduleHobbsTime = schedulerVM.AircraftEquipmentHobbsTimeList.Where(p => p.AircraftEquipmentTimeId == aircraftEquipmentTimeVM.Id).FirstOrDefault();

                if (aircraftScheduleHobbsTime != null)
                {
                    aircraftEquipmentTimeVM.TotalHours = aircraftScheduleHobbsTime.TotalTime;
                    aircraftEquipmentTimeVM.Hours = aircraftScheduleHobbsTime.OutTime;
                }
            }
        }

        public void CloseCheckInForm()
        {
            if (IsOpenFromContextMenu)
            {
               CloseDialog();
            }
            else
            {
                uiOptions.IsDisplayMainForm = true;
                uiOptions.IsDisplayCheckInForm = false;
            }
        }

       public void OpenForm()
        {
            if (schedulerVM.ScheduleActivityId.GetValueOrDefault() != 0)
            {
                OnActivityTypeValueChanged(schedulerVM.ScheduleActivityId);
            }

            uiOptions.IsDisplayForm = true;
            uiOptions.IsDisplayMainForm = true;
            uiOptions.IsDisplayCheckInForm = false;

            base.StateHasChanged();
        }

        private void ManageValues()
        {
            if (!schedulerVM.IsDisplayMember2Dropdown)
            {
                schedulerVM.Member2Id = null;
            }

            if (!uiOptions.IsDisplayFlightInfo)
            {
                schedulerVM.FlightRules = "";
                schedulerVM.FlightType = "";
            }

            if (!uiOptions.IsDisplayFlightRoutes)
            {
                schedulerVM.FlightRoutes = "";
            }

            if (!uiOptions.IsDisplayInstructor)
            {
                schedulerVM.InstructorId = null;
            }
        }

        public void OnActivityTypeValueChanged(long? value)
        {
            InitializeValues();

            if (value == null)
            {
                return;
            }

            if (value == (int)DataModels.Enums.ScheduleActivityType.CharterFlight)
            {
                schedulerVM.IsDisplayMember2Dropdown = true;
                uiOptions.IsDisplayFlightRoutes = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.RenterFlight)
            {
                schedulerVM.IsDisplayMember2Dropdown = true;
                uiOptions.IsDisplayFlightRoutes = true;
                uiOptions.IsDisplayFlightInfo = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.TourFlight)
            {
                schedulerVM.IsDisplayMember2Dropdown = true;
                uiOptions.IsDisplayFlightRoutes = true;
                uiOptions.IsDisplayFlightInfo = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.StudentSolo)
            {
                uiOptions.IsDisplayFlightRoutes = true;
                uiOptions.IsDisplayFlightInfo = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.Maintenance)
            {
                uiOptions.IsDisplayRecurring = false;
                uiOptions.IsDisplayStandBy = false;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.DiscoveryFlight)
            {
                uiOptions.IsDisplayInstructor = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.DualFlightTraining)
            {
                uiOptions.IsDisplayInstructor = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.GroundTraining)
            {
                //IsDisplayAircraftDropDown = false;
                uiOptions.IsDisplayInstructor = true;
            }

            schedulerVM.ScheduleActivityId = value;
            base.StateHasChanged();
        }

        public async Task OnCompanyValueChanged(int value)
        {
            schedulerVM.CompanyId = value;

            if (value != 0)
            {
                ChangeLoaderVisibilityAction(true);

                SchedulerVM schedulerVMData = await AircraftSchedulerService.GetDetailsByCompanyIdAsync(dependecyParams, schedulerVM.Id, value);

                ChangeLoaderVisibilityAction(false);

                schedulerVM.Member1List = schedulerVMData.Member1List;
                schedulerVM.Member2List = schedulerVMData.Member2List;
                schedulerVM.InstructorsList = schedulerVMData.InstructorsList;
                schedulerVM.AircraftsList = schedulerVMData.AircraftsList;
            }
        }

        public async Task CheckOutAircraft()
        {
            await SetCheckOutButtonState(true);

            // check if someone else already checked out it 

            CurrentResponse response = await AircraftSchedulerDetailService.IsAircraftAlreadyCheckOutAsync(dependecyParams, schedulerVM.AircraftId.GetValueOrDefault());

            await SetCheckOutButtonState(false);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if ((bool)response.Data == true)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);

                    if (IsOpenFromContextMenu)
                    {
                        CloseDialog();
                    }
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

        private async Task CheckOut()
        {
            CurrentResponse response = await AircraftSchedulerDetailService.CheckOut(dependecyParams, schedulerVM.Id);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = true;
                RefreshSchedulerDataSource(ScheduleOperations.CheckOut);
                CloseDialog();
            }
        }

        private async Task SetCheckOutButtonState(bool isBusy)
        {
            uiOptions.IsBusyCheckOutButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
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
                RefreshSchedulerDataSource(ScheduleOperations.UnCheckOut);
                CloseDialog();
            }
        }

        private void RefreshSchedulerDataSource(ScheduleOperations scheduleOperations)
        {
            RefreshSchedulerDataSourceParentEvent.InvokeAsync(scheduleOperations);
        }

        private void DeleteEvent()
        {
            DeleteParentEvent.InvokeAsync();
        }

        private void CloseChildDialog()
        {
            isDisplayChildPopup = false;
            OpenDialog();
        }

        public async Task DeleteAsync()
        {
            await SetDeleteButtonState(true);

            CurrentResponse response = await AircraftSchedulerService.DeleteAsync(dependecyParams, schedulerVM.Id);

            await SetDeleteButtonState(false);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog();
            }

            DeleteEvent();
        }

        private async Task SetDeleteButtonState(bool isBusy)
        {
            uiOptions.IsBusyDeleteButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        async Task GetDepartureAirportsList(AutoCompleteReadEventArgs args)
        {
            if (schedulerVM.DepartureAirport == null)
            {
                return;
            }

            schedulerVM.DepartureAirport = schedulerVM.DepartureAirport.ToUpper();

            if (schedulerVM.DepartureAirport.Length > 1)
            {
                args.Data = departureAirportList = await GetAirportsList(schedulerVM.DepartureAirport);
            }
        }

        async Task GetArrivalAirportsList(AutoCompleteReadEventArgs args)
        {
            if (schedulerVM.ArrivalAirport == null)
            {
                return;
            }

            schedulerVM.ArrivalAirport = schedulerVM.ArrivalAirport.ToUpper();

            if (schedulerVM.ArrivalAirport.Length > 1)
            {
                args.Data = arrivalAirpotList = await GetAirportsList(schedulerVM.ArrivalAirport);
            }
        }

        private async Task<List<DropDownGuidValues>> GetAirportsList(string airportName)
        {
            if (airportName == null)
            {
                return new List<DropDownGuidValues>();
            }

            airportAPIFilter.AirportCode = airportName.ToUpper();
            var data = await AirportService.ListDropDownValues(dependecyParams, airportAPIFilter);

            return data;
        }

        public void InitializeValues()
        {
            uiOptions.IsDisplayRecurring = true;
            uiOptions.IsDisplayMember1Dropdown = true;
            uiOptions.IsDisplayAircraftDropDown = true;
            schedulerVM.IsDisplayMember2Dropdown = false;
            uiOptions.IsDisplayFlightRoutes = false;
            uiOptions.IsDisplayInstructor = false;
            uiOptions.IsDisplayFlightInfo = false;
            uiOptions.IsDisplayStandBy = true;

            base.StateHasChanged();
        }

        public void OpenMainForm()
        {
            uiOptions.IsDisplayForm = true;
            uiOptions.IsDisplayCheckOutOption = false;
            uiOptions.IsDisplayMainForm = true;

            if (IsOpenFromContextMenu)
            {
                CloseDialog();
            }

            base.StateHasChanged();
        }

        public async Task LoadDataAsync()
        {
            await LoadDataParentEvent.InvokeAsync();
        }

        public async Task OpenAirportDetailsPopup(string airportName)
        {
            ChangeLoaderVisibilityAction(true);

            CurrentResponse response = await AirportService.FindByName(dependecyParams, airportName);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                AirportViewModel airports = JsonConvert.DeserializeObject<AirportViewModel>(response.Data.ToString());

                airportDetails = airports.Value.FirstOrDefault();

                childPopupTitle = "Airport Details";
                operationType = OperationType.DocumentViewer;
                isDisplayChildPopup = true;
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }

            ChangeLoaderVisibilityAction(false);
        }

        private void HideEditEndTimeForm()
        {
            uiOptions.IsDisplayEditEndTimeForm = false;
            uiOptions.IsDisplayMainForm = true;

            if (IsOpenFromContextMenu)
            {
                CloseDialog();
            }
        }
    }
}
