using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.ExternalAPI.Airport;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Web.UI.Models.Scheduler;
using Web.UI.Utilities;

namespace Web.UI.Pages.Scheduler
{
    partial class DetailsView
    {
        #region Paramters

        [Parameter] public SchedulerVM schedulerVM { get; set; }
        [Parameter] public UIOptions uiOptions { get; set; }
        [Parameter] public bool isAllowToEdit { get; set; }
        [Parameter] public bool isAllowToDelete { get; set; }

        [Parameter] public EventCallback CloseDialogParentEvent { get; set; }
        [Parameter] public EventCallback OpenMainFormParentEvent { get; set; }
        [Parameter] public EventCallback OpenDeleteDialogParentEvent { get; set; }
        [Parameter] public EventCallback OpenFormParentEvent { get; set; }
        [Parameter] public EventCallback ShowEditEndTimeFormParentEvent { get; set; }
        [Parameter] public EventCallback EditFlightTimeParentEvent { get; set; }
        [Parameter] public EventCallback CheckOutAircraftParentEvent { get; set; }
        [Parameter] public EventCallback OpenUnCheckOutDialogParentEvent { get; set; }
        [Parameter] public EventCallback CheckInAircraftParentEvent { get; set; }

        #endregion

        AirportDetailsViewModel airportDetails = new();
        DependecyParams dependecyParams;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
        }
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

        #region Parent Methods
        public async Task CloseDialog()
        {
           await CloseDialogParentEvent.InvokeAsync();
        }

        public async Task OpenMainForm()
        {
            await OpenMainFormParentEvent.InvokeAsync();
        }

        public async Task OpenForm()
        {
            await OpenFormParentEvent.InvokeAsync();
        }

        public async Task OpenDeleteDialog()
        {
            await OpenDeleteDialogParentEvent.InvokeAsync();
        }

        public async Task ShowEditEndTimeForm()
        {
            await ShowEditEndTimeFormParentEvent.InvokeAsync();
        }

        public async Task EditFlightTime()
        {
            await EditFlightTimeParentEvent.InvokeAsync();
        }

        public async Task OpenUnCheckOutDialog()
        {
            await OpenUnCheckOutDialogParentEvent.InvokeAsync();
        }

        public async Task CheckOutAircraft()
        {
            await CheckOutAircraftParentEvent.InvokeAsync();
        }

        public async Task CheckInAircraft()
        {
            await CheckInAircraftParentEvent.InvokeAsync();
        }

        #endregion

    }
}
