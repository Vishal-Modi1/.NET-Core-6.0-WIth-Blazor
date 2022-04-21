﻿using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using DataModels.VM.MyAccount;
using Microsoft.AspNetCore.Components;
using DataModels.VM.UserPreference;

namespace FSM.Blazor.Data.MyAccount
{
    public class MyAccountService
    {
        private readonly HttpCaller _httpCaller;

        public MyAccountService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(navigationManager, authenticationStateProvider);
        }

        public async Task<CurrentResponse> ChangePassword(IHttpClientFactory httpClient, ChangePasswordVM changePasswordVM)
        {
            string url = "myaccount/changepassword";
            string jsonData = JsonConvert.SerializeObject(changePasswordVM);
            
            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> AddMyPreference(IHttpClientFactory httpClient, UserPreferenceVM userPreferenceVM)
        {
            string url = "userpreference/create";
            string jsonData = JsonConvert.SerializeObject(userPreferenceVM);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }
    }
}
