using DataModels.Constants;
using DataModels.Entities;
using DataModels.Enums;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Web.UI.Data.Aircraft;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;

namespace Web.UI.Pages.Aircraft
{
    partial class AircraftDetails
    {
        public string AircraftId { get; set; }

        [Parameter]
        public AircraftVM aircraftData { get; set; }

        public string CompanyName;

        public bool isDataLoaded = false, isBusy = false, isUpdateButtonBusy = false, isDisplayLoader;
        string moduleName = "Aircraft";
        public bool isAllowToEdit, isUnLocked;
        DataModels.Enums.UserRole userRole;
        string modelWidth = "600px";

        protected override Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);
            SetSelectedMenuItem(moduleName);
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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
                aircraftData = await AircraftService.GetDetailsAsync(dependecyParams, Convert.ToInt64(AircraftId));

                try
                {
                    if (!string.IsNullOrEmpty(aircraftData.ImagePath))
                    {
                        var webClient = new WebClient();
                        byte[] imageBytes = webClient.DownloadData(aircraftData.ImagePath);

                        aircraftData.ImagePath = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    }
                }
                catch (Exception e)
                {

                }

                long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
                bool isCreator = userId == aircraftData.CreatedBy;

                bool isOwner = userId == aircraftData.OwnerId;

                if (globalMembers.IsAdmin || globalMembers.IsSuperAdmin || isCreator)
                {
                    isAllowToEdit = true;
                }

                isUnLocked = globalMembers.IsAdmin || globalMembers.IsSuperAdmin || isOwner || !aircraftData.IsLock;

                SetCompanyName();

                ChangeLoaderVisibilityAction(false);
                base.StateHasChanged();
            }
        }

        async Task AircraftEditDialog()
        {
            isBusy = true;
            operationType = OperationType.Edit;
            popupTitle = "Edit Aircraft Details";
            modelWidth = "600px";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            aircraftData = await AircraftService.GetDetailsAsync(dependecyParams, aircraftData.Id);

            SetCompanyName();

            isBusy = false;
            isDisplayPopup = true;
        }

        async Task OpenStatusUpdateDialog()
        {
            isUpdateButtonBusy = true;
            operationType = OperationType.UpdateStatus;
            modelWidth = "400px";

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            aircraftData = await AircraftService.GetDetailsAsync(dependecyParams, aircraftData.Id);

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
            aircraftData.AircraftStatusId = Convert.ToByte(id);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftStatusService.GetById(dependecyParams, aircraftData.AircraftStatusId);

            if (response.Status == HttpStatusCode.OK)
            {
                aircraftData.AircraftStatus = JsonConvert.DeserializeObject<AircraftStatus>(response.Data.ToString());
            }
        }

        private void SetCompanyName()
        {
            CompanyName = aircraftData.CompanyName;

            if (string.IsNullOrEmpty(CompanyName))
            {
                CompanyName = aircraftData.Companies.Where(p => p.Id == aircraftData.CompanyId).FirstOrDefault().Name;
            }
        }
    }
}
