﻿using DataModels.VM.Common;
using DE = DataModels.Entities;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using FSM.Blazor.Utilities;

namespace FSM.Blazor.Pages.AircraftMake
{
    public partial class Create
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Parameter] public DE.AircraftMake AircraftMake { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftMakeService.SaveandUpdateAsync(dependecyParams, AircraftMake);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Make", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Make", response.Message);
                NotificationService.Notify(message);
            }

        }
        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
