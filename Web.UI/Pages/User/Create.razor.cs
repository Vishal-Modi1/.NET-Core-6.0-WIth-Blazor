using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Constants;
using DataModels.Enums;
using DataModels.Models;
using Web.UI.Models.Shared;

namespace Web.UI.Pages.User
{
    public partial class Create
    {
        [Parameter] public UserVM userData { get; set; }

        [Parameter] public bool FromAuthoriseSection { get; set; }

        [Parameter] public Action SaveBothStepsValue { get; set; }

        [Parameter] public Action GoBackStep { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        //TODO:
        //public TelerikEditForm<UserVM> userForm;
        bool isInstructorTypeDropdownVisible = false, isAuthenticated = false;
        int? roleId;
        List<RadioButtonItem> genderOptions { get; set; } = new List<RadioButtonItem>
        {
            new RadioButtonItem { Id = 0,Text = "Female" },
            new RadioButtonItem { Id = 1, Text = "Male" },
        };

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

            if (!isAuthenticated && !userData.IsInvited)
            {
                userData.RoleId = userData.UserRoles.Where(p => p.Name == UserRole.Owner.ToString()).First().Id;
            }

            if (userData.Id == 0)
            {
                userData.IsSendEmailInvite = userData.IsSendTextMessage = true;
            }
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            CurrentResponse response;
            bool isEmailExist = false;

            if (userData.Id == 0)
            {
                isBusySubmitButton = true;

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                response = await UserService.IsEmailExistAsync(dependecyParams, userData.Email);

                isBusySubmitButton = false;

                isEmailExist = ManageIsEmailExistResponse(response);
            }

            if (!isEmailExist)
            {
                isBusySubmitButton = true;

                userData.ActivationLink = NavigationManager.BaseUri + "AccountActivation?Token=";

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                response = await UserService.SaveandUpdateAsync(dependecyParams, userData);

                isBusySubmitButton = false;

                ManageResponse(response, "User Details", true);
            }
        }

        async void OnGenderValueChange()
        {
            if (userData.GenderId == 0)
            {
                userData.Gender = "Male";
            }
            else
            {
                userData.Gender = "Female";
            }
        }

        async void OnIsInstructorValueChange()
        {
            if (userData.IsInstructor)
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
            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            
        }

        private bool ManageIsEmailExistResponse(CurrentResponse response)
        {
            bool isEmailExist = false;
            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                isEmailExist = true;
            }

            return isEmailExist;
        }

        async void GotoSave()
        {
            //TODO:
            //if (!userForm.EditContext.Validate())
            //{
            //    return;
            //}

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

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
