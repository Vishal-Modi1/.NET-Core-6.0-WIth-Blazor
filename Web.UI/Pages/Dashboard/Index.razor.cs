﻿using DataModels.VM.Common;
using DataModels.VM.User;
using Web.UI.Data.User;
using Web.UI.Extensions;
using Web.UI.Shared;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Telerik.Blazor.Components;
using Web.UI.Models.Enums;

namespace Web.UI.Pages.Dashboard
{
    public partial class Index
    {
        [CascadingParameter] protected Notification Notification { get; set; }

        public UserVM userData { get; set; }
        public bool isPopup { get; set; }
       
        #region Objects
       
        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [CascadingParameter]
        public MainLayout Layout { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        private bool isDisplayLoader { get; set; } = false;
        UserVM userVM = new UserVM();
        DateTime DateofBirth { get; set; } = DateTime.Now;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            base.StateHasChanged();
            await LoadData();
        }

        async Task LoadData()
        {
            isDisplayLoader = true;
            NotificationModel message;

            var user = (await AuthStat).User;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.FindById(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                message = new NotificationModel().Build(TelerikNotificationTypes.info, "Something went Wrong!, Please try again later.");
                Notification.Instance.Show(message);
            }

            userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());

            isDisplayLoader = false;
        }
    }
}