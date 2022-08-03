using DataModels.Constants;
using DataModels.Entities;
using DataModels.Enums;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using FSM.Blazor.Data.Aircraft;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
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

        public bool isDataLoaded = false, isBusy = false, isUpdateButtonBusy = false, isDisplayLoader;
        private CurrentUserPermissionManager _currentUserPermissionManager;
        string moduleName = "Aircraft", popupTitle;
        public bool isAllowToEdit, isDisplayPopup;
        OperationType operationType = OperationType.Edit;
        DataModels.Enums.UserRole userRole;
        string modelWidth = "600px";

        protected override async Task OnInitializedAsync()
        {
            isDisplayLoader = true;

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            userRole = _currentUserPermissionManager.GetRole(AuthStat).Result;

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


        async Task AircraftEditDialog(long id, string title)
        {
            isBusy = true;
            operationType = OperationType.Edit;
            modelWidth = "600px";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            AircraftData = await AircraftService.GetDetailsAsync(dependecyParams, id);

            SetCompanyName();

            isBusy = false;
            isDisplayPopup = true;
            popupTitle = title;
        }

        async Task OpenStatusUpdateDialog(long id)
        {
            isUpdateButtonBusy = true;
            operationType = OperationType.UpdateStatus;
            modelWidth = "400px";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            AircraftData = await AircraftService.GetDetailsAsync(dependecyParams, id);

            isUpdateButtonBusy = false;
            isDisplayPopup = true;
            popupTitle = "Update Status";
        }

        async Task CloseDialog()
        {
            isDisplayPopup = false;
        }

        async Task UpdateStatus(int id)
        {
            AircraftData.AircraftStatusId = Convert.ToByte(id);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftStatusService.GetById(dependecyParams, AircraftData.AircraftStatusId);

            if (response != null && response.Status == HttpStatusCode.OK)
            {
                AircraftData.AircraftStatus = JsonConvert.DeserializeObject<AircraftStatus>(response.Data.ToString());
            }
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
