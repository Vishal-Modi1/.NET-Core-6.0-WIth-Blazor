using DataModels.VM.Common;
using DataModels.VM.User;
using FSM.Blazor.Data.User;
using FSM.Blazor.Extensions;
using FSM.Blazor.Shared;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Radzen;

namespace FSM.Blazor.Pages.Dashboard
{
    public partial class Index
    {
        public UserVM userData { get; set; }
        public bool isPopup { get; set; }
       
        #region Objects
       
        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [CascadingParameter]
        public MainLayout Layout { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        [Inject] UserService UserService { get; set; }
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

            var user = (await AuthStat).User;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.FindById(dependecyParams);

            NotificationMessage message;

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }

            userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());

            isDisplayLoader = false;
        }
    }
}
