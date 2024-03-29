﻿using DataModels.VM.Common;
using DataModels.VM.Location;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView.Settings.Location
{
    public partial class Create
    {
        [Parameter] public LocationVM locationData { get; set; }
        [Parameter] public Action GoToNext { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        int timezoneId;

        protected override Task OnInitializedAsync()
        {
            timezoneId = locationData.TimezoneId;
            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = false;
            locationData.TimezoneId = (short)timezoneId;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LocationService.SaveandUpdateAsync(dependecyParams, locationData);

            isBusySubmitButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
