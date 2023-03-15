using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using Web.UI.Utilities;
using DataModels.VM.Reservation;
using Web.UI.Models.Constants;
using Utilities;

namespace Web.UI.Pages.Company.DetailsView
{
    partial class Index
    {
        public string CompanyId { get; set; }
        public CompanyVM companyData { get; set; } = new();
        string moduleName = Module.Company.ToString();
        public bool isAllowToEdit;
        public List<UpcomingFlight> upcomingFlights = new();
        DependecyParams dependecyParams;

        protected override async Task OnInitializedAsync()
        {
            SetSelectedMenuItem(moduleName);
            ChangeLoaderVisibilityAction(true);

            companyData = new();
            companyData.PrimaryServicesList = new List<DropDownValues>();

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("CompanyId", out link);

            if (link.Count() == 0 || link[0] == "")
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
            CompanyId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace(UpflyteConstant.QuesryString, "");

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.GetDetailsAsync(dependecyParams, Convert.ToInt32(CompanyId));

            if (response.Status == HttpStatusCode.OK)
            {
                companyData = JsonConvert.DeserializeObject<CompanyVM>(response.Data.ToString());
                companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);
            }

            try
            {
                if (!string.IsNullOrEmpty(companyData.Logo))
                {
                    var webClient = new WebClient();
                    byte[] imageBytes = await webClient.DownloadDataTaskAsync(new Uri(companyData.LogoPath));

                    companyData.LogoPath = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                }
            }
            catch (Exception e)
            {

            }

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
            bool isCreator = userId == companyData.CreatedBy;

            if (globalMembers.IsAdmin || globalMembers.IsSuperAdmin || isCreator)
            {
                isAllowToEdit = true;
            }

            await LoadUpcomingFlights();

            ChangeLoaderVisibilityAction(false);
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
                
        //    }
        //}

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            try
            {
                string fileType = Path.GetExtension(e.File.Name);
                List<string> supportedImagesFormatsList = supportedImagesFormats?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (supportedImagesFormatsList is not null && !supportedImagesFormatsList.Contains(fileType))
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "File type is not supported");
                    return;
                }

                if (e.File.Size > maxProfileImageUploadSize)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, $"File size exceeds maximum limit { maxProfileImageUploadSize / (1024 * 1024) } MB.");
                    return;
                }

                MemoryStream ms = new MemoryStream();
                await e.File.OpenReadStream(maxProfileImageUploadSize).CopyToAsync(ms);
                byte[] bytes = ms.ToArray();

                await OnChangeAsync(bytes);
            }
            catch (Exception ex)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, ex.ToString());
            }
        }

        void CompanyEditDialog()
        {
            popupTitle = "Edit Details";
            isDisplayPopup = true;
        }

        void CloseDialog()
        {
            isDisplayPopup = false;
        }

        async Task OnChangeAsync(byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(companyData.LogoPath))
            {
                return;
            }

             ChangeLoaderVisibilityAction(true);

            //byte[] bytes = Convert.FromBase64String(companyData.LogoPath.Substring(companyData.LogoPath.IndexOf(",") + 1));

            ByteArrayContent data = new ByteArrayContent(bytes);

            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent
                {
                   { data, "0","0" }
                };

                string companyId = companyData.Id == null ? "0" : companyData.Id.ToString();
                multiContent.Add(new StringContent(companyId), "CompanyId");

                CurrentResponse response = await CompanyService.UploadLogo(dependecyParams, multiContent);

                ManageFileUploadResponse(response, true, bytes);
            }
            catch (Exception ex)
            {

            }

             ChangeLoaderVisibilityAction(false);
        }

        private void ManageFileUploadResponse(CurrentResponse response, bool isCloseDialog, byte[] byteArray)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == HttpStatusCode.OK && isCloseDialog)
            {
                var b64String = Convert.ToBase64String(byteArray);
                companyData.Logo = "data:image/png;base64," + b64String;
                CloseDialog();
            }
        }

        private async Task LoadUpcomingFlights()
        {
            upcomingFlights = await ReservationService.ListUpcomingFlightsByCompanyId(dependecyParams, companyData.Id);
            upcomingFlights.ForEach(p =>
            {
                p.StartDate = DateConverter.ToLocal(p.StartDate, globalMembers.Timezone);

            });
        }
    }
}
