using Newtonsoft.Json;
using System.Security.Claims;
using DataModels.VM.UserRolePermission;
using DataModels.Enums;
using DataModels.Constants;
using Microsoft.Extensions.Caching.Memory;
using DataModels.VM.Common;

namespace FSM.Blazor.Utilities
{
    public class CurrentUserPermissionManager
    {
        private static IHttpContextAccessor _accessor;
        private static HttpContext _httpContext => _accessor.HttpContext;
        private static IMemoryCache _memoryCache => _httpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache;
        private static HttpCaller _httpCaller;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;
            _httpCaller = new HttpCaller();
        }

        public static void AddInCache(int loggedUserId, List<UserRolePermissionDataVM> userRolePermissionsList)
        {
            _memoryCache.Set(loggedUserId, userRolePermissionsList);
        }

        public static async Task<List<UserRolePermissionDataVM>> GetAsync()
        {
            ClaimsPrincipal cp = _httpContext.User;

            string claimValue = cp.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                               .Select(c => c.Value).SingleOrDefault();

             //cookie value has been cleare
            if(string.IsNullOrWhiteSpace(claimValue))
            {
                return new List<UserRolePermissionDataVM>();
            }

            int loggedUserId = Convert.ToInt32(claimValue);
            List<UserRolePermissionDataVM> userRolePermissionsList;

            bool isExist = _memoryCache.TryGetValue(loggedUserId, out userRolePermissionsList);

            if(!isExist)
            {
                // Cache has been clear, need to call database for get the permission list

                //CurrentResponse response = await _httpCaller.GetAsync($"UserRolePermission/listbyroleid", _httpClient);
                //userRolePermissionsList = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data);
                
                //if (userRolePermissionsList != null && userRolePermissionsList.Count() > 0)
                //{
                //    AddInCache(loggedUserId, userRolePermissionsList);
                //}
            }

            return userRolePermissionsList;
        }

        public static bool IsAllowed(PermissionType permissionType, string moduleName)
        {
            List<UserRolePermissionDataVM> userRolePermissionsList = GetAsync().Result;

            bool isAllowed = userRolePermissionsList.Where(p => p.IsAllowed == true &&
                              p.ModuleName.ToLower() == moduleName && p.PermissionType == permissionType.ToString()).Count() > 0;

            return isAllowed;
        }

        public static bool IsSuperAdmin()
        {
            ClaimsPrincipal cp = _httpContext.User;

            string claimValue = cp.Claims.Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value).SingleOrDefault();

            return Convert.ToInt32(claimValue) == ((int)UserRole.SuperAdmin);
        }
    }
}
