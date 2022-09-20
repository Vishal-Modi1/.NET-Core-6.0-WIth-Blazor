using DataModels.VM.Common;
using DataModels.VM.MyAccount;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.RegularExpressions;

namespace Web.UI.Pages.MyAccount
{
    partial class ChangePassword
    {
        public string id { get; set; }
        private ChangePasswordVM changePasswordVM = new ChangePasswordVM();

        private int _passwordStrengthCounter;
        private const string UpperCasePattern = @"(?=.*[A-Z])";
        private const string LowerCasePattern = @"(?=.*[a-z])";
        private const string DigitPattern = @"(?=.*\d)";
        private const string SpecialCharacterPattern = @"(?=.*[-+_!@#$%^&*., ?])";

        protected override async Task OnInitializedAsync()
        {
            changePasswordVM.NewPassword = String.Empty;
            var user = (await AuthStat).User;

            if (!user.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/Login");
            }

            id = user.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                          .Select(c => c.Value).SingleOrDefault();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            changePasswordVM.UserId = Convert.ToInt64(id);
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await MyAccountService.ChangePassword(dependecyParams, changePasswordVM);

            isBusySubmitButton = false;
            StateHasChanged();

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            changePasswordVM = new ChangePasswordVM();
            changePasswordVM.UserId = Convert.ToInt64(id);
        }

        private bool PasswordValid()
        {
            _passwordStrengthCounter = 0;
            bool passwordValid = false;
            if (changePasswordVM.NewPassword is not null)
            {
                bool passwordLengthValid = changePasswordVM.NewPassword.Length >= 8;

                if (Regex.Match(changePasswordVM.NewPassword, LowerCasePattern).Success)
                {
                    _passwordStrengthCounter++;
                }

                if (Regex.Match(changePasswordVM.NewPassword, UpperCasePattern).Success)
                {
                    _passwordStrengthCounter++;
                }

                if (Regex.Match(changePasswordVM.NewPassword, DigitPattern).Success)
                {
                    _passwordStrengthCounter++;
                }

                if (Regex.Match(changePasswordVM.NewPassword, SpecialCharacterPattern).Success && _passwordStrengthCounter < 3)
                {
                    _passwordStrengthCounter++;
                }

                if (_passwordStrengthCounter >= 3 && passwordLengthValid)
                {
                    passwordValid = true;
                }

                if (passwordLengthValid)
                {
                    _passwordStrengthCounter++;
                }

                if (changePasswordVM.NewPassword != changePasswordVM.ConfirmPassword)
                {
                    passwordValid = false;
                }

            }
            return passwordValid;
        }
    }
}
