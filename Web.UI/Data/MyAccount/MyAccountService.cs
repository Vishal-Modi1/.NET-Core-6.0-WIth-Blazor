using DataModels.VM.Common;
using DataModels.VM.MyAccount;
using DataModels.VM.UserPreference;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.MyAccount
{
    public class MyAccountService
    {
        private readonly HttpCaller _httpCaller;

        public MyAccountService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> ChangePassword(DependecyParams dependecyParams, ChangePasswordVM changePasswordVM)
        {
            dependecyParams.URL = "myaccount/changepassword";
            dependecyParams.JsonData = JsonConvert.SerializeObject(changePasswordVM);
            
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> AddMyPreference(DependecyParams dependecyParams, UserPreferenceVM userPreferenceVM)
        {
            dependecyParams.URL = "userpreference/create";
            dependecyParams.JsonData = JsonConvert.SerializeObject(userPreferenceVM);

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }
    }
}
