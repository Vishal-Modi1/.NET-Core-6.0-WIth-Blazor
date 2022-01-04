﻿using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using DataModels.VM.Account;

namespace FSM.Blazor.Data.Account
{
    public class AccountService
    {
        private readonly HttpCaller _httpCaller;

        public AccountService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> ForgetPasswordAsync(IHttpClientFactory httpClient, ForgotPasswordVM forgotPasswordVM)
        {
            string jsonData = JsonConvert.SerializeObject(forgotPasswordVM);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, "account/forgotpassword", jsonData);

            return response;
        }

        public async Task<CurrentResponse> ValidateResetPasswordTokenAsync(IHttpClientFactory httpClient, string token)
        {
            CurrentResponse response = await _httpCaller.GetAsync(httpClient, $"account/validatetoken?token={token}");

            return response;
        }

        public async Task<CurrentResponse> ResetPasswordAsync(IHttpClientFactory httpClient, ResetPasswordVM resetPasswordVM)
        {
            string jsonData = JsonConvert.SerializeObject(resetPasswordVM);
            CurrentResponse response = await _httpCaller.PostAsync(httpClient, "account/resetpassword", jsonData);


            return response; 
        }
    }
}