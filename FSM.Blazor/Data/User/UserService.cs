using DataModels.VM.User;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using DataModels.VM.UserPreference;

namespace FSM.Blazor.Data.User
{
    public class UserService
    {
        private readonly HttpCaller _httpCaller;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<UserDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "user/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<UserDataVM>();
            }

            List<UserDataVM> userDataList = JsonConvert.DeserializeObject<List<UserDataVM>>(response.Data.ToString());

            return userDataList;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, UserVM userVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(userVM);

            dependecyParams.URL = "user/create";

            if (userVM.Id > 0)
            {
                dependecyParams.URL = "user/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"user/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UpdateIsUserActive(DependecyParams dependecyParams, long id, bool isActive)
        {
            dependecyParams.URL = $"user/updatestatus?id={id}&isActive={isActive}";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<UserVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"user/getDetails?id={id}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            UserVM userVM = new UserVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
            }

            return userVM;
        }

        public async Task<UserVM> GetMasterDetailsAsync(DependecyParams dependecyParams, bool isInvited, string token)
        {
            token = token == "" ? "dummy token" : token;
            dependecyParams.URL = $"user/getMasterDetails?isInvited={isInvited}&token={token}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            UserVM userVM = new UserVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
            }

            return userVM;
        }

        public async Task<UserFilterVM> GetFiltersAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"user/getfilters";
            var response = await _httpCaller.GetAsync(dependecyParams);

            UserFilterVM userFilterVM = new UserFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userFilterVM = JsonConvert.DeserializeObject<UserFilterVM>(response.Data.ToString());
            }

            return userFilterVM;
        }

        public async Task<CurrentResponse> IsEmailExistAsync(DependecyParams dependecyParams, string email)
        {
            dependecyParams.URL = $"user/isemailexist?email={email}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> FindById(DependecyParams dependecyParams)
        {
            long id = Convert.ToInt64(_httpCaller.GetClaimValue("Id", dependecyParams.AuthenticationStateProvider));

            dependecyParams.URL = $"user/findbyid?id={id}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<List<UserPreferenceVM>> FindMyPreferencesById(DependecyParams dependecyParams)
        {
            long id = Convert.ToInt64(_httpCaller.GetClaimValue("Id", dependecyParams.AuthenticationStateProvider));

            dependecyParams.URL = $"user/findmypreferencesbyid?id={id}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            List<UserPreferenceVM> listUserPreferences = new List<UserPreferenceVM>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                listUserPreferences = JsonConvert.DeserializeObject<List<UserPreferenceVM>>(response.Data.ToString());
            }

            return listUserPreferences;
        }

        public async Task<CurrentResponse> UploadProfileImageAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"user/uploadprofileimage";

            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }

        public async Task<List<DropDownLargeValues>> ListDropDownValuesByCompanyId(DependecyParams dependecyParams, int companyId)
        {
            dependecyParams.URL = $"user/listdropdownvaluesbycompanyid?companyId={companyId}";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownLargeValues> usersList = new List<DropDownLargeValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                usersList = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return usersList;
        }
    }
}
