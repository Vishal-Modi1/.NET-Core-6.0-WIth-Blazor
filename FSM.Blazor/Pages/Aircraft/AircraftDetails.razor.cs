using DataModels.Constants;
using DataModels.VM.Aircraft;
using FSM.Blazor.Data.Aircraft;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Radzen;
using System.Net;

namespace FSM.Blazor.Pages.Aircraft
{
    partial class AircraftDetails
    {
        public string AircraftId { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter]
        public AircraftVM AircraftData { get; set; }

        public string CompanyName;

        public bool isDataLoaded = false, isBusy = false, isDisplayLoader;
        private CurrentUserPermissionManager _currentUserPermissionManager;
        string moduleName = "Aircraft";
        public bool isAllowToEdit;

        protected override async Task OnInitializedAsync()
        {
            isDisplayLoader = true;

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("AircraftId", out link);

            if (link.Count() == 0 || link[0] == "")
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
            AircraftId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace("FlyManager", "");

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            AircraftData = await AircraftService.GetDetailsAsync(dependecyParams, Convert.ToInt64(AircraftId));

            try
            {
                if (!string.IsNullOrEmpty(AircraftData.ImagePath))
                {
                    var webClient = new WebClient();
                    byte[] imageBytes = webClient.DownloadData(AircraftData.ImagePath);

                    AircraftData.ImagePath = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception e)
            {

            }

            bool isAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.Admin).Result;
            bool isSuperAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.SuperAdmin).Result;

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);

            bool isCreator = userId == AircraftData.CreatedBy;

            if (isAdmin || isSuperAdmin || isCreator)
            {
                isAllowToEdit = true;
            }

            isDisplayLoader = false;

            SetCompanyName();
        }

        async Task AircraftEditDialog(int id, string title)
        {
            isBusy = true;
            StateHasChanged();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            AircraftVM aircraftData = await AircraftService.GetDetailsAsync(dependecyParams, id);

            isBusy = false;
            StateHasChanged();

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "aircraftData", AircraftData } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            SetCompanyName();
        }

        private void SetCompanyName()
        {
            CompanyName = AircraftData.CompanyName;

            if (string.IsNullOrEmpty(CompanyName))
            {
                CompanyName = AircraftData.Companies.Where(p => p.Id == AircraftData.CompanyId).FirstOrDefault().Name;
            }
        }
    }
}
