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
        [Parameter] public EventCallback CloseDialogParentEvent { get; set; }
        [Parameter] public EventCallback OpenDialogParentEvent { get; set; }
        [Parameter] public EventCallback LoadDataParentEvent { get; set; }

        #endregion

        string timezone = "";

        public bool isAllowToEdit;
        public bool isAllowToDelete;
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

        EditContext checkInForm;

        protected override async Task OnInitializedAsync()
        {
            timezone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            // user should be superadmin, admin or owner of reservation to update or delete it

            bool isAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.Admin).Result;
            bool isSuperAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.SuperAdmin).Result;

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);

            bool isCreator = userId == schedulerVM.CreatedBy;

            if (isAdmin || isSuperAdmin || isCreator)
            {
                isAllowToDelete = true;
                isAllowToEdit = true;
            }

            checkInForm = new EditContext(schedulerVM);
        }

        private async void OnValidSubmit()
        {
            uiOptions.isDisplayCheckOutOption = false;

            if (uiOptions.isDisplayForm)
            {
                ManageValues();
                uiOptions.isDisplayForm = false;
                base.StateHasChanged();

                return;
            }

            isBusySubmitButton = true;

            //schedulerVM.StartTime = DateConverter.ToUTC(schedulerVM.StartTime, timezone);
            //schedulerVM.EndTime = DateConverter.ToUTC(schedulerVM.EndTime, timezone);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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

            CloseDialog();
            await LoadDataAsync();
        }

        void OpenUnCheckOutDialog()
        {
            childPopupTitle = "Un-Check out appointment";
            isDisplayChildPopup = true;
            operationType = OperationType.UnCheckOut;
        }

        void OpenDeleteDialog()
        {
            childPopupTitle = "Delete Appointment";
            isDisplayChildPopup = true;
            operationType = OperationType.Delete;
        }


        private void CloseDialog()
        {
            CloseDialogParentEvent.InvokeAsync();
        }

        private void OpenDialog()
        {
            OpenDialogParentEvent.InvokeAsync();
        }

        private async Task CheckInAircraft()
        {
            uiOptions.isDisplayMainForm = false;
        }

        private async Task HideEditEndTimeForm()
        {
            uiOptions.isDisplayEditEndTimeForm = false;
            uiOptions.isDisplayMainForm = true;
        }

        private async Task ShowEditEndTimeForm()
        {
            uiOptions.isDisplayMainForm = false;
            uiOptions.isDisplayEditEndTimeForm = true;
        }

        private async Task UpdateEndTime()
        {
            isBusySubmitButton = true;

            SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM = new SchedulerEndTimeDetailsVM();

            schedulerEndTimeDetailsVM.ScheduleId = schedulerVM.Id;
            schedulerEndTimeDetailsVM.EndTime = DateConverter.ToUTC(schedulerVM.EndTime, timezone);
            schedulerEndTimeDetailsVM.StartTime = DateConverter.ToUTC(schedulerVM.StartTime, timezone);
            schedulerEndTimeDetailsVM.AircraftId = schedulerVM.AircraftId.GetValueOrDefault();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftSchedulerService.UpdateScheduleEndTime(dependecyParams, schedulerEndTimeDetailsVM);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data) == true)
                {
                    uiOptions.isDisplayEditEndTimeForm = false;
                    uiOptions.isDisplayMainForm = true;
                    globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, response.Message);
                    RefreshSchedulerDataSource(ScheduleOperations.UpdateEndTime);
                    base.StateHasChanged();
                }
                else
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
                }
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }

            isBusySubmitButton = false;
            base.StateHasChanged();
        }

        private async Task EditFlightTime()
        {
            uiOptions.isDisplayMainForm = false;

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

        private async Task CheckIn()
        {
            if (!checkInForm.Validate())
            {
                return;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftSchedulerDetailService.CheckIn(dependecyParams, schedulerVM.AircraftEquipmentsTimeList);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog();
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = false;

                RefreshSchedulerDataSource(ScheduleOperations.CheckIn);
            }
        }

        private void OpenForm()
        {
            if (schedulerVM.ScheduleActivityId.GetValueOrDefault() != 0)
            {
                OnActivityTypeValueChanged(schedulerVM.ScheduleActivityId);
            }

            uiOptions.isDisplayForm = true;
            base.StateHasChanged();
        }

        private void ManageValues()
        {
            if (!schedulerVM.IsDisplayMember2Dropdown)
            {
                schedulerVM.Member2Id = null;
            }

            if (!uiOptions.isDisplayFlightInfo)
            {
                schedulerVM.FlightRules = "";
                schedulerVM.FlightType = "";
            }

            if (!uiOptions.isDisplayFlightRoutes)
            {
                schedulerVM.FlightRoutes = "";
            }

            if (!uiOptions.isDisplayInstructor)
            {
                schedulerVM.InstructorId = null;
            }
        }

        public void OnActivityTypeValueChanged(int? value)
        {
            InitializeValues();

            if (value == null)
            {
                return;
            }

            if (value == (int)DataModels.Enums.ScheduleActivityType.CharterFlight)
            {
                schedulerVM.IsDisplayMember2Dropdown = true;
                uiOptions.isDisplayFlightRoutes = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.RenterFlight)
            {
                schedulerVM.IsDisplayMember2Dropdown = true;
                uiOptions.isDisplayFlightRoutes = true;
                uiOptions.isDisplayFlightInfo = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.TourFlight)
            {
                schedulerVM.IsDisplayMember2Dropdown = true;
                uiOptions.isDisplayFlightRoutes = true;
                uiOptions.isDisplayFlightInfo = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.StudentSolo)
            {
                uiOptions.isDisplayFlightRoutes = true;
                uiOptions.isDisplayFlightInfo = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.Maintenance)
            {
                uiOptions.isDisplayRecurring = false;
                uiOptions.isDisplayMember1Dropdown = false;
                schedulerVM.Member1Id = 0;
                uiOptions.isDisplayStandBy = false;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.DiscoveryFlight)
            {
                uiOptions.isDisplayInstructor = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.DualFlightTraining)
            {
                uiOptions.isDisplayInstructor = true;
            }

            else if (value == (int)DataModels.Enums.ScheduleActivityType.GroundTraining)
            {
                //IsDisplayAircraftDropDown = false;
                uiOptions.isDisplayInstructor = true;
            }

            schedulerVM.ScheduleActivityId = value;
            base.StateHasChanged();
        }

        private async Task CheckOutAircraft()
        {
            await SetCheckOutButtonState(true);

            // check if someone else already checked out it 

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftSchedulerDetailService.IsAircraftAlreadyCheckOutAsync(dependecyParams, schedulerVM.AircraftId.GetValueOrDefault());

            await SetCheckOutButtonState(false);

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

        private async Task CheckOut()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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
            uiOptions.isBusyCheckOutButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        public void TextBoxChangeEvent(decimal value, int index)
        {
            schedulerVM.AircraftEquipmentsTimeList[index].TotalHours = value - schedulerVM.AircraftEquipmentsTimeList[index].Hours;
            schedulerVM.AircraftEquipmentsTimeList[index].InTime = value;
        }

        public void EditFlightTimeTextBoxChangeEvent(decimal value, int index)
        {
            //if (string.IsNullOrWhiteSpace(value.ToString()))
            //{
            //    return;
            //}

            schedulerVM.AircraftEquipmentsTimeList[index].TotalHours = value - schedulerVM.AircraftEquipmentsTimeList[index].Hours;
            schedulerVM.AircraftEquipmentHobbsTimeList[index].InTime = value;
            schedulerVM.AircraftEquipmentsTimeList[index].InTime = value;

            base.StateHasChanged();
        }

        //private BadgeStyle GetSchedulerStatusClass()
        //{
        //    if (schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut)
        //    {
        //        return BadgeStyle.Success;
        //    }
        //    else if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
        //    {
        //        return BadgeStyle.Light;
        //    }
        //    else
        //    {
        //        return BadgeStyle.Primary;
        //    }
        //    //    int SchedulerStatus = 1;
        //    //switch (SchedulerStatus)
        //    //{
        //    //    case 1:
        //    //        //success
        //    //        return "badge-primary";
        //    //    default:
        //    //        return string.Empty;
        //    //}
        //    //<span class="badge badge-primary">Primary</span>
        //    //<span class="badge badge-secondary">Secondary</span>
        //    //<span class="badge badge-success">Success</span>
        //    //<span class="badge badge-danger">Danger</span>
        //    //<span class="badge badge-warning">Warning</span>
        //    //<span class="badge badge-info">Info</span>
        //    //<span class="badge badge-light">Light</span>
        //    //<span class="badge badge-dark">Dark</span>
        //}

        private string GetSchedulerStatusText()
        {
            if (schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut)
            {
                return "Checked Out";
            }
            else if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
            {
                return "Checked In";
            }
            else
            {
                return "Scheduled";
            }
        }

        private async Task UnCheckOutAppointment()
        {
            uiOptions.isBusyUnCheckOutButton = true; 

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftSchedulerDetailService.UnCheckOut(dependecyParams, schedulerVM.AircraftSchedulerDetailsVM.Id);

            uiOptions.isBusyUnCheckOutButton = false;

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

        async Task DeleteAsync()
        {
            await SetDeleteButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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
            uiOptions.isBusyDeleteButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        public void InitializeValues()
        {
            uiOptions.isDisplayRecurring = true;
            uiOptions.isDisplayMember1Dropdown = true;
            uiOptions.isDisplayAircraftDropDown = true;
            schedulerVM.IsDisplayMember2Dropdown = false;
            uiOptions.isDisplayFlightRoutes = false;
            uiOptions.isDisplayInstructor = false;
            uiOptions.isDisplayFlightInfo = false;
            uiOptions.isDisplayStandBy = true;

            base.StateHasChanged();
        }

        public void OpenMainForm()
        {
            uiOptions.isDisplayForm = true;
            uiOptions.isDisplayCheckOutOption = false;
            uiOptions.isDisplayMainForm = true;

            base.StateHasChanged();
        }

        public async Task LoadDataAsync()
        {
            await LoadDataParentEvent.InvokeAsync();
        }
    }
}
