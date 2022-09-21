using DataModels.VM.UserRolePermission;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using DataModels.VM.Common;
using Microsoft.JSInterop;

namespace Web.UI.Data.UserRolePermission
{
    public class UserRolePermissionService
    {
        private readonly HttpCaller _httpCaller;

        public UserRolePermissionService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<UserRolePermissionFilterVM> GetFiltersAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "userrolepermission/getfilters";
            var response = await _httpCaller.GetAsync(dependecyParams);

            UserRolePermissionFilterVM userFilterVM = new UserRolePermissionFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userFilterVM = JsonConvert.DeserializeObject<UserRolePermissionFilterVM>(response.Data.ToString());
            }

            return userFilterVM;
        }

        public async Task<List<UserRolePermissionDataVM>> ListAsync(DependecyParams dependecyParams, UserRolePermissionDatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "userrolepermission/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<UserRolePermissionDataVM>();
            }

            List<UserRolePermissionDataVM> userrolePermissions = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data.ToString());

            return userrolePermissions;
        }

        public async Task<CurrentResponse> UpdatePermissionAsync(DependecyParams dependecyParams, long id, bool isAllow, bool isForWeb)
        {
            dependecyParams.URL = $"userrolepermission/updatepermission?id={id}&isAllow={isAllow}";

            if(!isForWeb)
            {
                dependecyParams.URL = $"userrolepermission/updatemobileapppermission?id={id}&isAllow={isAllow}";
            }

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UpdatePermissionsAsync(DependecyParams dependecyParams, UserRolePermissionFilterVM userRolePermissionFilterVM, bool isForWeb)
        {
            dependecyParams.URL = $"userrolepermission/updatepermissions";
            
            if (!isForWeb)
            {
                dependecyParams.URL = $"userrolepermission/updatemobileapppermissions";
            }

            dependecyParams.JsonData = JsonConvert.SerializeObject(userRolePermissionFilterVM);
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }
    }
}
