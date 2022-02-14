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

namespace FSM.Blazor.Pages.Scheduler
{
    partial class Create
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        [Parameter]
        public SchedulerVM schedulerVM { get; set; }

        [Parameter]
        public UIOptions uiOptions { get; set; }
        
        [Parameter]
        public EventCallback InitializeValuesParentEvent { get; set; }

        [Parameter]
        public EventCallback DeleteParentEvent { get; set; }

        [Parameter]
        public EventCallback<bool?> RefreshSchedulerDataSourceParentEvent { get; set; }

        [Parameter]
        public EventCallback CloseDialogParentEvent { get; set; }

        public bool isBusy;

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

            isBusy = true;

            CurrentResponse response = await AircraftSchedulerService.SaveandUpdateAsync(_httpClient, schedulerVM);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (response.Data == null)
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                    NotificationService.Notify(message);
                }
                else
                {
                    //dialogVisibility = false;
                    message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                    NotificationService.Notify(message);
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }

            isBusy = false;

            //   await LoadDataAsync();
        }

        private void CloseDialog()
        {
            CloseDialogParentEvent.InvokeAsync();
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
            isBusy = true;

            NotificationMessage message;

            SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM = new SchedulerEndTimeDetailsVM();

            schedulerEndTimeDetailsVM.ScheduleId = schedulerVM.Id;
            schedulerEndTimeDetailsVM.EndTime = schedulerVM.EndTime;
            schedulerEndTimeDetailsVM.StartTime = schedulerVM.StartTime;
            schedulerEndTimeDetailsVM.AircraftId = schedulerVM.AircraftId.GetValueOrDefault();

            CurrentResponse response = await AircraftSchedulerService.UpdateScheduleEndTime(_httpClient, schedulerEndTimeDetailsVM);

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data) == true)
                {
                    uiOptions.isDisplayEditEndTimeForm = false;
                    uiOptions.isDisplayMainForm = true;
                    message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                    NotificationService.Notify(message);

                    RefreshSchedulerDataSource(null);
                    base.StateHasChanged();
                }
                else
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                    NotificationService.Notify(message);
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }

            isBusy = false;
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

        private async Task OpenMainForm()
        {
            uiOptions.isDisplayMainForm = true;
        }

        private async Task CheckIn()
        {
            NotificationMessage message;

            CurrentResponse response = await AircraftSchedulerDetailService.CheckIn(_httpClient, schedulerVM.AircraftEquipmentsTimeList);

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                uiOptions.dialogVisibility = false;
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                NotificationService.Notify(message);

                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = false;

                RefreshSchedulerDataSource(schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
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
            if (!uiOptions.isDisplayMember2Dropdown)
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

        public void OnActivityTypeValueChanged(object value)
        {
            InitializeValues();

            if (value == null)
            {
                return;
            }

            if ((int)value == (int)DataModels.Enums.ScheduleActivityType.CharterFlight)
            {
                uiOptions.isDisplayMember2Dropdown = true;
                uiOptions.isDisplayFlightRoutes = true;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.RenterFlight)
            {
                uiOptions.isDisplayMember2Dropdown = true;
                uiOptions.isDisplayFlightRoutes = true;
                uiOptions.isDisplayFlightInfo = true;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.TourFlight)
            {
                uiOptions.isDisplayMember2Dropdown = true;
                uiOptions.isDisplayFlightRoutes = true;
                uiOptions.isDisplayFlightInfo = true;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.StudentSolo)
            {
                uiOptions.isDisplayFlightRoutes = true;
                uiOptions.isDisplayFlightInfo = true;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.Maintenance)
            {
                uiOptions.isDisplayRecurring = false;
                uiOptions.isDisplayMember1Dropdown = false;
                schedulerVM.Member1Id = 0;
                uiOptions.isDisplayStandBy = false;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.DiscoveryFlight)
            {
                uiOptions.isDisplayInstructor = true;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.DualFlightTraining)
            {
                uiOptions.isDisplayInstructor = true;
            }

            else if ((int)value == (int)DataModels.Enums.ScheduleActivityType.GroundTraining)
            {
                //IsDisplayAircraftDropDown = false;
                uiOptions.isDisplayInstructor = true;
            }

            base.StateHasChanged();
        }

        private async Task CheckOutAircraft()
        {
            await SetCheckOutButtonState(true);

            // check if someone else already checked out it 
            CurrentResponse response = await AircraftSchedulerDetailService.IsAircraftAlreadyCheckOutAsync(_httpClient, schedulerVM.AircraftId.GetValueOrDefault());

            await SetCheckOutButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if ((bool)response.Data == true)
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                    NotificationService.Notify(message);
                }
                else
                {
                    await CheckOut();
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }

        }

        private async Task CheckOut()
        {
            NotificationMessage message;

            CurrentResponse response = await AircraftSchedulerDetailService.CheckOut(_httpClient, schedulerVM.Id);

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                uiOptions.dialogVisibility = false;
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                NotificationService.Notify(message);

                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = true;
                RefreshSchedulerDataSource(schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        private async Task SetCheckOutButtonState(bool isBusy)
        {
            uiOptions.isBusyCheckOutButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        public void TextBoxChangeEvent(ChangeEventArgs args, int index)
        {
            schedulerVM.AircraftEquipmentsTimeList[index].TotalHours = Convert.ToDecimal(args.Value) - schedulerVM.AircraftEquipmentsTimeList[index].Hours;

            base.StateHasChanged();
        }

        public void EditFlightTimeTextBoxChangeEvent(ChangeEventArgs value, int index)
        {
            schedulerVM.AircraftEquipmentsTimeList[index].TotalHours = Convert.ToDecimal(value.Value) - schedulerVM.AircraftEquipmentsTimeList[index].Hours;
            schedulerVM.AircraftEquipmentHobbsTimeList[index].InTime = Convert.ToDecimal(value.Value);

            base.StateHasChanged();
        }

        private string GetSchedulerStatusClass()
        {
            int SchedulerStatus = 1;
            switch (SchedulerStatus)
            {
                case 1:
                    //success
                    return "badge-primary";
                default:
                    return string.Empty;
            }
            //<span class="badge badge-primary">Primary</span>
            //<span class="badge badge-secondary">Secondary</span>
            //<span class="badge badge-success">Success</span>
            //<span class="badge badge-danger">Danger</span>
            //<span class="badge badge-warning">Warning</span>
            //<span class="badge badge-info">Info</span>
            //<span class="badge badge-light">Light</span>
            //<span class="badge badge-dark">Dark</span>
        }

        private string GetSchedulerStatusText()
        {

            int SchedulerStatus = 1;
            switch (SchedulerStatus)
            {
                case 1:
                    return "success";
                default:
                    return string.Empty;
            }
        }

        private async Task UnCheckOutAppointment()
        {
            NotificationMessage message;

            await SetUnCheckOutButtonState(true);

            CurrentResponse response = await AircraftSchedulerDetailService.UnCheckOut(_httpClient, schedulerVM.AircraftSchedulerDetailsVM.Id);

            await SetUnCheckOutButtonState(false);

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Appointment Details", response.Message);
                NotificationService.Notify(message);

                schedulerVM.AircraftSchedulerDetailsVM = new AircraftSchedulerDetailsVM();
                base.StateHasChanged();

                RefreshSchedulerDataSource(false);
                DialogService.Close(true);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Appointment Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        private void RefreshSchedulerDataSource(bool? isCheckOut)
        {
            RefreshSchedulerDataSourceParentEvent.InvokeAsync(isCheckOut);
        }

        private void DeleteEvent()
        {
            DeleteParentEvent.InvokeAsync();
        }

        private void CloseChildDialog()
        {
            DialogService.Close(true);
            uiOptions.dialogVisibility = true;
        }

        private async Task SetUnCheckOutButtonState(bool isBusy)
        {
            uiOptions.isBusyUnCheckOutButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
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

            DeleteEvent();
        }

        private async Task SetDeleteButtonState(bool isBusy)
        {
            uiOptions.isBusyDeleteButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        public void InitializeValues()
        {
            InitializeValuesParentEvent.InvokeAsync();
        }
    }
}
