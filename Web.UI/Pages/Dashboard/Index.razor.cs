using DataModels.VM.Common;
using DataModels.VM.User;
using Web.UI.Data.User;
using Web.UI.Extensions;
using Web.UI.Shared;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Telerik.Blazor.Components;
using static Telerik.Blazor.ThemeConstants;

namespace Web.UI.Pages.Dashboard
{
    public partial class Index
    {
        public UserVM userData { get; set; }
       
        UserVM userVM = new UserVM();
        DateTime DateofBirth { get; set; } = DateTime.Now;

        protected override async Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            await LoadData();
        }

        async Task LoadData()
        {
            var user = (await AuthStat).User;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.FindById(dependecyParams);

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                uiNotification.DisplayErrorNotification(uiNotification.Instance);
                return;
            }

            userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());

            ChangeLoaderVisibilityAction(false);
        }
    }
}
