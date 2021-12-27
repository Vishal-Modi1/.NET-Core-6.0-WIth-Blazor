using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;


namespace FSM.Blazor.Pages.User
{
    public partial class Create
    {
        [Parameter]
        public UserVM userData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isInstructorTypeDropdownVisible = false;

        protected override void OnInitialized()
        {
            isInstructorTypeDropdownVisible = userData.IsInstructor;
        }

        public async Task Submit(UserVM userDataVM)
        {
            CurrentResponse response;
            bool isEmailExist = false;

            if (userDataVM.Id == 0)
            {
                response = await UserService.IsEmailExistAsync(_httpClient, userData.Email);
                isEmailExist = ManageIsEmailExistResponse(response, "", false);
            }

            if (!isEmailExist)
            {
                response = await UserService.SaveandUpdateAsync(_httpClient, userDataVM);
                ManageResponse(response, "User Details", true);
            }
        }

        async void OnGenderValueChange(int value)
        {
            if (value == 0)
            {
                userData.Gender = "Male";
            }
            else
            {
                userData.Gender = "Female";
            }
        }

        async void OnIsInstructorValueChange(bool value)
        {
            if (value)
            {
                isInstructorTypeDropdownVisible = true;
            }
            else
            {
                isInstructorTypeDropdownVisible = false;
            }
        }

        private void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (isCloseDialog)
                {
                    dialogService.Close(true);
                }

                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }
        }

        private bool ManageIsEmailExistResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;
            bool isEmailExist = false;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isEmailExist = true;
                    message = new NotificationMessage().Build(NotificationSeverity.Error, summary, "Email is already exist!");
                    NotificationService.Notify(message);
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }

            return isEmailExist;
        }
    }
}
