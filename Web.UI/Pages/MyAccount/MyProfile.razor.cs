using DataModels.VM.Common;
using DataModels.VM.User;
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
        bool isBusySubmitButton { get; set; } = false;


        void SetCurrentActiveInfo2Tab(string activeInfoTab)
        {
            if (currentActiveInfo2Tab != activeInfoTab)
            {
                currentActiveInfo2Tab = activeInfoTab;
            }
        }
        void DummyAction() { }

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

        private void ManageFileUploadResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog();
            }
        }

        void CloseDialog()
        {
            isDisplayPopup = false;
        }

        async Task OnChangeAsync()
        {
            if (string.IsNullOrWhiteSpace(userVM.ImageName))
            {
                return;
            }

            isDisplayLoader = true;

            byte[] bytes = Convert.FromBase64String(userVM.ImageName.Substring(userVM.ImageName.IndexOf(",") + 1));

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

                ManageFileUploadResponse(response, "Profile Image updated successfully.", true);
            }
            catch (Exception ex)
            {

            }

            isDisplayLoader = false;
        }
    }
}
