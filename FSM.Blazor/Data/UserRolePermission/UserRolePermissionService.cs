using DataModels.VM.UserRolePermission;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using DataModels.VM.Common;

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
                userFilterVM = JsonConvert.DeserializeObject<UserRolePermissionFilterVM>(response.Data);
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

            List<UserRolePermissionDataVM> userrolePermissions = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data);

            return userrolePermissions;
        }

        public async Task<CurrentResponse> UpdatePermissionAsync(IHttpClientFactory httpClient, int id, bool isAllow)
        {
            CurrentResponse response = await _httpCaller.GetAsync(httpClient,$"userrolepermission/updatepermission?id={id}&isAllow={isAllow}");

            return response;
        }

        public async Task<CurrentResponse> UpdateMultiplePermissionsAsync(IHttpClientFactory httpClient, UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            string jsonData = JsonConvert.SerializeObject(userRolePermissionFilterVM);
            CurrentResponse response = await _httpCaller.PostAsync(httpClient, $"userrolepermission/updatemultiplepermissions", jsonData);

            return response;
        }
    }
}
