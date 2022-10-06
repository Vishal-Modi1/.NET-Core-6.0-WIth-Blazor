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
using Microsoft.AspNetCore.Components.Forms;

namespace Web.UI.Pages.User
{
    public partial class Create
    {
        [Parameter] public UserVM userData { get; set; }

        [Parameter] public bool FromAuthoriseSection { get; set; }

        [Parameter] public Action SaveBothStepsValue { get; set; }

        [Parameter] public Action GoBackStep { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        EditContext userForm;
        bool isInstructorTypeDropdownVisible = false, isLoggedIn;
        int? roleId;
       
        List<RadioButtonItem> genderOptions { get; set; } = new List<RadioButtonItem>
        {
            new RadioButtonItem { Id = 0,Text = "Male" },
            new RadioButtonItem { Id = 1, Text = "Female" },
        };

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            userForm = new EditContext(userData);

            if (userData != null)
            {
                isInstructorTypeDropdownVisible = userData.IsInstructor;
                OnGenderValueChange();
            }
            else
            {
                userData = new UserVM();
            }

            isLoggedIn = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result) ;
            userData.IsAuthenticated = (!string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result) && userData.RoleId != (int)UserRole.SuperAdmin);

            if (!isLoggedIn && !userData.IsInvited)
            {
                userData.RoleId = userData.UserRoles.Where(p => p.Name == UserRole.Owner.ToString()).First().Id;
            }

            if (userData.Id == 0)
            {
                userData.IsSendEmailInvite = userData.IsSendTextMessage = true;
            }

            userData.ExistingCompanyId = userData.CompanyId;
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            CurrentResponse response;
            bool isEmailExist = false;

            if (userData.UserPreferences != null)
            {
                userData.UserPreferences.Clear();
            }

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

        void OnGenderValueChange()
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
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        private bool ManageIsEmailExistResponse(CurrentResponse response)
        {
            bool isEmailExist = false;
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status != System.Net.HttpStatusCode.OK)
            {
                isEmailExist = true;
            }

            return isEmailExist;
        }

        async void GotoSave()
        {
            if (!userForm.Validate())
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

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
