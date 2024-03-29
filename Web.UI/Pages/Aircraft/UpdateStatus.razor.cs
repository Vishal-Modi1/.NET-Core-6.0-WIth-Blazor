﻿using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Aircraft
{
    partial class UpdateStatus
    {
        [Parameter] public AircraftVM aircraftData { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public EventCallback<int> UpdateStatusCallBack { get; set; }
        
        public int aircraftStatusId;

        protected override Task OnInitializedAsync()
        {
            aircraftStatusId = aircraftData.AircraftStatusId;
            return base.OnInitializedAsync();
        }

        async Task Update()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftService.UpdateStatus(dependecyParams, aircraftData.Id, Convert.ToByte(aircraftStatusId));
            ManageResponse(response, "", true);
        }

        private async void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                aircraftData.AircraftStatusId = Convert.ToByte(aircraftStatusId);
                CloseDialog(true);
                UpdateUI();
            }
        }

        public void CloseDialog(bool refreshData)
        {
            CloseDialogCallBack.InvokeAsync(refreshData);
        }

        public void UpdateUI()
        {
            UpdateStatusCallBack.InvokeAsync(aircraftData.AircraftStatusId);
        }
    }
}
