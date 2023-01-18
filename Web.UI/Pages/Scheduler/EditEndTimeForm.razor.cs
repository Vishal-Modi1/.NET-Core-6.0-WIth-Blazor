using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Utilities;
using Web.UI.Models.Scheduler;
using Web.UI.Utilities;

namespace Web.UI.Pages.Scheduler
{
    partial class EditEndTimeForm
    {
        [Parameter] public SchedulerVM schedulerVM { get; set; }
        [Parameter] public UIOptions uiOptions { get; set; }
        [Parameter] public bool IsOpenFromContextMenu { get; set; }
        [Parameter] public EventCallback CloseDialogParentEvent { get; set; }
        [Parameter] public EventCallback HideEditEndTimeFormParentEvent { get; set; }
        [Parameter] public EventCallback<ScheduleOperations> RefreshSchedulerDataSourceParentEvent { get; set; }
       
        DependecyParams dependecyParams;
        string timezone;

        protected override async Task OnInitializedAsync()
        {
            timezone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
        }

        private async Task UpdateEndTime()
        {
            isBusySubmitButton = true;

            SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM = new SchedulerEndTimeDetailsVM();

            schedulerEndTimeDetailsVM.ScheduleId = schedulerVM.Id;
            schedulerEndTimeDetailsVM.EndTime = DateConverter.ToUTC(schedulerVM.EndTime, timezone);
            schedulerEndTimeDetailsVM.StartTime = DateConverter.ToUTC(schedulerVM.StartTime, timezone);
            schedulerEndTimeDetailsVM.AircraftId = schedulerVM.AircraftId.GetValueOrDefault();

            CurrentResponse response = await AircraftSchedulerService.UpdateScheduleEndTime(dependecyParams, schedulerEndTimeDetailsVM);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data) == true)
                {
                    uiOptions.IsDisplayEditEndTimeForm = false;
                    uiOptions.IsDisplayMainForm = true;
                    globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, response.Message);
                    RefreshSchedulerDataSource(ScheduleOperations.UpdateEndTime);

                    if (IsOpenFromContextMenu)
                    {
                        CloseDialog();
                    }

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

        private void HideEditEndTimeForm()
        {
            HideEditEndTimeFormParentEvent.InvokeAsync();
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
