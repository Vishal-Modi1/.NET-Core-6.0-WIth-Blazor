using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Utilities;
using Web.UI.Models.Scheduler;
using Web.UI.Utilities;

namespace Web.UI.Pages.Scheduler
{
    partial class EditCheckInForm
    {
        [Parameter] public SchedulerVM schedulerVM { get; set; }
        [Parameter] public UIOptions uiOptions { get; set; }
        [Parameter] public bool IsOpenFromContextMenu { get; set; }
        [Parameter] public EventCallback CloseDialogParentEvent { get; set; }
        [Parameter] public EventCallback CloseCheckInFormParentEvent { get; set; }
        [Parameter] public EventCallback<ScheduleOperations> RefreshSchedulerDataSourceParentEvent { get; set; }

        DependecyParams dependecyParams;
        EditContext checkInForm;

        protected override async Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            checkInForm = new EditContext(schedulerVM);
        }

        private async Task CheckIn()
        {
            if (!checkInForm.Validate())
            {
                return;
            }

            isBusySubmitButton = true;
            CurrentResponse response = await AircraftSchedulerDetailService.CheckIn(dependecyParams, schedulerVM.AircraftEquipmentsTimeList);
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog();
                schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut = false;

                RefreshSchedulerDataSource(ScheduleOperations.CheckIn);
            }

            isBusySubmitButton = false;
        }

        public void TextBoxChangeEvent(decimal value, int index)
        {
            schedulerVM.AircraftEquipmentsTimeList[index].TotalHours = value - schedulerVM.AircraftEquipmentsTimeList[index].Hours;
            schedulerVM.AircraftEquipmentsTimeList[index].InTime = value;
        }

        public void EditFlightTimeTextBoxChangeEvent(decimal value, int index)
        {
            schedulerVM.AircraftEquipmentsTimeList[index].TotalHours = value - schedulerVM.AircraftEquipmentsTimeList[index].Hours;
            schedulerVM.AircraftEquipmentHobbsTimeList[index].InTime = value;
            schedulerVM.AircraftEquipmentsTimeList[index].InTime = value;

            base.StateHasChanged();
        }

        private void CloseCheckInForm()
        {
            CloseCheckInFormParentEvent.InvokeAsync();
        }

        private void CloseDialog()
        {
            CloseDialogParentEvent.InvokeAsync();
        }

        private void RefreshSchedulerDataSource(ScheduleOperations scheduleOperations)
        {
            RefreshSchedulerDataSourceParentEvent.InvokeAsync(scheduleOperations);
        }
    }
}
