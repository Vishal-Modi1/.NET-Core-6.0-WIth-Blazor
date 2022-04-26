using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using DataModels.VM.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FSM.Blazor.Data.Account
{
    public class AccountService
    {
        private readonly HttpCaller _httpCaller;

        public AccountService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> ForgetPasswordAsync(DependecyParams dependecyParams, ForgotPasswordVM forgotPasswordVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(forgotPasswordVM);
            dependecyParams.URL = "account/forgotpassword";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> ValidateResetPasswordTokenAsync(DependecyParams dependecyParams, string token)
        {
            dependecyParams.URL = $"account/validatetoken?token={token}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> ResetPasswordAsync(DependecyParams dependecyParams, ResetPasswordVM resetPasswordVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(resetPasswordVM);
            dependecyParams.URL = "account/resetpassword";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);


            return response; 
        }

        public async Task<CurrentResponse> ActivateAccountAsync(DependecyParams dependecyParams, string token)
        {
            dependecyParams.URL = $"account/activateaccount?token={token}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            
            return response;
        }
    }
}
