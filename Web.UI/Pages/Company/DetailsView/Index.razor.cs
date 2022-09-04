using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView
{
    partial class Index
    {
        public string CompanyId { get; set; }
        public CompanyVM companyData { get; set; }
        string moduleName = Module.Company.ToString();
        public bool isAllowToEdit;
      
        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            StringValues link;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("CompanyId", out link);

            if (link.Count() == 0 || link[0] == "")
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
            CompanyId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace("FlyManager", "");

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.GetDetailsAsync(dependecyParams, Convert.ToInt32(CompanyId));

            companyData = new CompanyVM();

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

            bool isAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.Admin).Result;
            bool isSuperAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, DataModels.Enums.UserRole.SuperAdmin).Result;

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);

            bool isCreator = userId == companyData.CreatedBy;

            if (isAdmin || isSuperAdmin || isCreator)
            {
                isAllowToEdit = true;
            }

            isDisplayLoader = false;
            
        }

        void CompanyEditDialog()
        {
            isDisplayPopup = true;
        }

        void CloseDialog()
        {
            isDisplayPopup = false;
        }

        async Task OnChangeAsync()
        {
            if (string.IsNullOrWhiteSpace(companyData.LogoPath))
            {
                return;
            }

            isDisplayLoader = true;

            byte[] bytes = Convert.FromBase64String(companyData.LogoPath.Substring(companyData.LogoPath.IndexOf(",") + 1));

            ByteArrayContent data = new ByteArrayContent(bytes);

            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent
                {
                   { data, "0","0" }
                };

                string companyId = companyData.Id == null ? "0" : companyData.Id.ToString();
                multiContent.Add(new StringContent(companyId), "CompanyId");

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await CompanyService.UploadLogo(dependecyParams, multiContent);

                ManageFileUploadResponse(response, "Profile Image updated successfully.", true);
            }
            catch (Exception ex)
            {

            }

            isDisplayLoader = false;
        }

        private void ManageFileUploadResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK && isCloseDialog)
            {
                CloseDialog();
            }
        }
    }
}
