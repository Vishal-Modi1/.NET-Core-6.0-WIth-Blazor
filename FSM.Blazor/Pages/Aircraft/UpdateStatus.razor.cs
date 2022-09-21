using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;

namespace FSM.Blazor.Pages.Aircraft
{
    partial class UpdateStatus
    {
        [Parameter] public AircraftVM AircraftData { get; set; }

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public EventCallback<int> UpdateStatusCallBack { get; set; }

        public RadzenTemplateForm<AircraftVM> form;

        public int aircraftStatusId;
        bool isBusy = false;

        protected override Task OnInitializedAsync()
        {
            aircraftStatusId = AircraftData.AircraftStatusId;

            return base.OnInitializedAsync();
        }

        async Task Update(AircraftVM airCraftData)
        {
            isBusy = true;
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftService.UpdateStatus(dependecyParams, airCraftData.Id, (byte)aircraftStatusId);

            isBusy = false;

            ManageResponse(response, "", true);
        }

        private async void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(false);
                Update();
                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        public void Update()
        {
            UpdateStatusCallBack.InvokeAsync(aircraftStatusId);
        }
    }
}
