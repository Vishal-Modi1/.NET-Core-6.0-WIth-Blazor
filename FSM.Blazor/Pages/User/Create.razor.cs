using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using DataModels.Constants;
using DataModels.Enums;

namespace FSM.Blazor.Pages.User
{
    public partial class Create
    {
        [Parameter] public UserVM userData { get; set; }

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter] public Action SaveBothStepsValue { get; set; }

        [Parameter] public Action GoBackStep { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;
        public RadzenTemplateForm<UserVM> userForm;
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isInstructorTypeDropdownVisible = false, isAuthenticated = false;
        bool isBusy;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            isAuthenticated = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result);

            if (userData != null)
            {
                isInstructorTypeDropdownVisible = userData.IsInstructor;
            }
            else
            {
                userData = new UserVM();
            }

            if (!isAuthenticated)
            {
                userData.RoleId = userData.UserRoles.Where(p => p.Name == UserRole.Owner.ToString()).First().Id;
            }
            
            userData.IsSendEmailInvite = userData.IsSendTextMessage = true;
        }

        public async Task Submit(UserVM userDataVM)
        {
            SetButtonState(true);

            CurrentResponse response;
            bool isEmailExist = false;

            if (userDataVM.Id == 0)
            {
                SetButtonState(true);

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                response = await UserService.IsEmailExistAsync(dependecyParams, userData.Email);

                SetButtonState(false);

                isEmailExist = ManageIsEmailExistResponse(response, "", false);
            }

            if (!isEmailExist)
            {
                SetButtonState(true);

                userDataVM.ActivationLink = NavigationManager.BaseUri + "AccountActivation?Token=";

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                response = await UserService.SaveandUpdateAsync(dependecyParams, userDataVM);

                SetButtonState(false);

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
                CloseDialog(false);
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

        private void SetButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }

        async void GotoSave()
        {
            if (!userForm.EditContext.Validate())
            {
                return;
            }

            SaveBothStepsValue.Invoke();
        }

        async void GoBack()
        {
            GoBackStep.Invoke();
        }

        void RedirectToLogin()
        {
            NavigationManager.NavigateTo("/Login");
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
