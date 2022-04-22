using DataModels.VM.UserRolePermission;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DataModels.VM.Common;
using Microsoft.JSInterop;

namespace FSM.Blazor.Data.UserRolePermission
{
    public class UserRolePermissionService
    {
        private readonly HttpCaller _httpCaller;

        public UserRolePermissionService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<UserRolePermissionFilterVM> GetFiltersAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"userrolepermission/getfilters");

            UserRolePermissionFilterVM userFilterVM = new UserRolePermissionFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userFilterVM = JsonConvert.DeserializeObject<UserRolePermissionFilterVM>(response.Data.ToString());
            }

            return userFilterVM;
        }

        public async Task<List<UserRolePermissionDataVM>> ListAsync(IHttpClientFactory httpClient, UserRolePermissionDatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, "userrolepermission/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<UserRolePermissionDataVM>();
            }

            List<UserRolePermissionDataVM> userrolePermissions = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data.ToString());

            return userrolePermissions;
        }

        public async Task<CurrentResponse> UpdatePermissionAsync(IHttpClientFactory httpClient, long id, bool isAllow, bool isForWeb)
        {
            string url = $"userrolepermission/updatepermission?id={id}&isAllow={isAllow}";

            if(!isForWeb)
            {
                url = $"userrolepermission/updatemobileapppermission?id={id}&isAllow={isAllow}";
            }

            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);

            return response;
        }

        public async Task<CurrentResponse> UpdatePermissionsAsync(IHttpClientFactory httpClient, UserRolePermissionFilterVM userRolePermissionFilterVM, bool isForWeb)
        {
            string url = $"userrolepermission/updatepermissions";
            
            if (!isForWeb)
            {
                url = $"userrolepermission/updatemobileapppermissions";
            }
            string jsonData = JsonConvert.SerializeObject(userRolePermissionFilterVM);
            CurrentResponse response = await _httpCaller.PostAsync(httpClient,  url, jsonData);

            return response;
        }
    }
}
