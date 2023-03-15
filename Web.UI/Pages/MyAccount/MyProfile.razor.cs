using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Web.UI.Utilities;
using Web.UI.Models.Constants;

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
             ChangeLoaderVisibilityAction(true);

            var user = (await AuthStat).User;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await UserService.FindById(dependecyParams);

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
            }
            else
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
            }

             ChangeLoaderVisibilityAction(false);
        }

        public async Task OpenUpdateProfileDialog()
        {
            userVM.IsFromMyProfile = true;
            userVM.Country = userVM.Countries.Where(p => p.Id == userVM.CountryId).First().Name;

            isDisplayPopup = true;
        }

        private void ManageFileUploadResponse(CurrentResponse response, byte[] byteArray)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
                CloseDialog();

                var b64String = Convert.ToBase64String(byteArray);
                userVM.ImageName = globalMembers.UserImagePath = "data:image/png;base64," + b64String;
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
                List<string> supportedImagesFormatsList = supportedImagesFormats.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!supportedImagesFormatsList.Contains(fileType))
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

             ChangeLoaderVisibilityAction(true);

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

             ChangeLoaderVisibilityAction(false);
        }

        public async Task EditLogBookInfo(long id)
        {
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + UpflyteConstant.QuesryString);
            NavigationManager.NavigateTo("LogBook?LogBookId=" + System.Convert.ToBase64String(encodedBytes));
        }
    }
}
