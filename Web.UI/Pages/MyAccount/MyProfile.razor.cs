using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Pages.MyAccount
{
    partial class MyProfile
    {
        UserVM userVM = new UserVM();

        public string currentActiveInfoTab { get; set; } = "Personal Information";
        public string currentActiveInfo2Tab { get; set; } = "My Preference";

        public void SetCurrentActiveInfoTab(string activeInfoTab)
        {
            if (currentActiveInfoTab != activeInfoTab)
            {
                currentActiveInfoTab = activeInfoTab;
            }
        }

        bool IsInstructor { get; set; } = false;

        void SetCurrentActiveInfo2Tab(string activeInfoTab)
        {
            if (currentActiveInfo2Tab != activeInfoTab)
            {
                currentActiveInfo2Tab = activeInfoTab;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            await LoadData();
        }

        async Task LoadData()
        {
            isDisplayLoader = true;

            var user = (await AuthStat).User;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.FindById(dependecyParams);

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                uiNotification.DisplayErrorNotification(uiNotification.Instance);
            }
            else
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
            }

            isDisplayLoader = false;
        }

        public async Task OpenUpdateProfileDialog()
        {
            userVM.IsFromMyProfile = true;
            userVM.Country = userVM.Countries.Where(p => p.Id == userVM.CountryId).First().Name;

            isDisplayPopup = true;
        }

        private void ManageFileUploadResponse(CurrentResponse response, byte[] byteArray)
        {
            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog();

                var b64String = Convert.ToBase64String(byteArray);
                userVM.ImageName = "data:image/png;base64," + b64String;
            }
        }

        void CloseDialog()
        {
            isDisplayPopup = false;
        }

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            try
            {
                string fileType = Path.GetExtension(e.File.Name);
                List<string> supportedImagesFormatsList = supportedImagesFormat.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!supportedImagesFormatsList.Contains(fileType))
                {
                    uiNotification.DisplayCustomErrorNotification(uiNotification.Instance, "File type is not supported");
                    return;
                }

                if (e.File.Size > maxProfileImageUploadSize)
                {
                    uiNotification.DisplayCustomErrorNotification(uiNotification.Instance, $"File size exceeds maximum limit { maxProfileImageUploadSize / (1024 * 1024) } MB.");
                    return;
                }

                MemoryStream ms = new MemoryStream();
                await e.File.OpenReadStream(maxProfileImageUploadSize).CopyToAsync(ms);
                byte[] bytes = ms.ToArray();

                await OnChangeAsync(bytes);
            }
            catch (Exception ex)
            {
                uiNotification.DisplayCustomErrorNotification(uiNotification.Instance, ex.ToString());
            }
        }

        private async Task PopulateImageFromStream(Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] byteArray = ms.ToArray();
            var b64String = Convert.ToBase64String(byteArray);
            string imageURL = "data:image/png;base64," + b64String;
        }


        async Task OnChangeAsync(byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(userVM.ImageName))
            {
                return;
            }

            isDisplayLoader = true;

            //byte[] bytes = Convert.FromBase64String(userVM.ImageName.Substring(userVM.ImageName.IndexOf(",") + 1));

            ByteArrayContent data = new ByteArrayContent(bytes);

            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent
                {
                   { data, "0","0" }
                };

                string companyId = userVM.CompanyId == null ? "0" : userVM.CompanyId.ToString();

                multiContent.Add(new StringContent(userVM.Id.ToString()), "UserId");
                multiContent.Add(new StringContent(companyId), "CompanyId");

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await UserService.UploadProfileImageAsync(dependecyParams, multiContent);

                ManageFileUploadResponse(response, bytes);
            }
            catch (Exception ex)
            {

            }

            isDisplayLoader = false;
        }
    }
}
