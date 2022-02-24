using DataModels.VM.Common;
using DataModels.VM.User;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Radzen;
using UP = FSM.Blazor.Pages.User;

namespace FSM.Blazor.Pages.MyAccount
{
    partial class Index
    {
        #region Objects
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        UserVM userVM = new UserVM();

        private bool isDisplayLoader { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
            await LoadData();
        }

        async Task LoadData()
        {
            isDisplayLoader = true;

            CurrentResponse response = await UserService.FindById(_httpClient);

            NotificationMessage message;

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }

            userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());

            isDisplayLoader = false;
        }

        public async Task OpenUpdateProfileDialog()
        {
            userVM.IsFromMyProfile = true;

            await DialogService.OpenAsync<UP.Create>("Edit",
                  new Dictionary<string, object>() { { "userData", userVM } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            userVM.Country = userVM.Countries.Where(p => p.Id == userVM.CountryId).First().Name;
        }

        private void ManageFileUploadResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong while uploading file!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (isCloseDialog)
                {
                    DialogService.Close(true);
                }

                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, "");
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong while uploading file!", response.Message);
                NotificationService.Notify(message);
            }
        }

        async Task OnChangeAsync()
        {
            if(string.IsNullOrWhiteSpace(userVM.ImageName))
            {
                return;
            }

            isDisplayLoader = true; 

            byte[] bytes = Convert.FromBase64String(userVM.ImageName.Substring(userVM.ImageName.IndexOf(",") + 1));

            ByteArrayContent data = new ByteArrayContent(bytes);

            MultipartFormDataContent multiContent = new MultipartFormDataContent
                {
                   { data, "file", userVM.Id.ToString() }
                };

            CurrentResponse response = await UserService.UploadProfileImageAsync(_httpClient, multiContent);

            ManageFileUploadResponse(response, "Profile Image updated successfully.", true);

            isDisplayLoader = false;
        }
    }
}
