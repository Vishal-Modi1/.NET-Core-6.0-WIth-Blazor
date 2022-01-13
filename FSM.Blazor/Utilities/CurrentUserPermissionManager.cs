using System.Security.Claims;
using DataModels.VM.UserRolePermission;
using DataModels.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Constants;

namespace FSM.Blazor.Utilities
{
    public class CurrentUserPermissionManager
    {
        private readonly IMemoryCache _memoryCache;
        private static CurrentUserPermissionManager _instance = null;
        private static readonly object padlock = new object();

        public static CurrentUserPermissionManager GetInstance(IMemoryCache memoryCache)
        {

            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new CurrentUserPermissionManager(memoryCache);
                }

                return _instance;
            }
        }


        private CurrentUserPermissionManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddInCache(long loggedUserId, List<UserRolePermissionDataVM> userRolePermissionsList)
        {
            _memoryCache.Set(loggedUserId, userRolePermissionsList);
        }

        public async Task<List<UserRolePermissionDataVM>> GetAsync(Task<AuthenticationState> authenticationState)
        {
            var cp = (await authenticationState).User;

            string claimValue = cp.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                               .Select(c => c.Value).SingleOrDefault();

            //cookie value has been cleare
            if (string.IsNullOrWhiteSpace(claimValue))
            {
                return new List<UserRolePermissionDataVM>();
            }

            long loggedUserId = Convert.ToInt64(claimValue);
            List<UserRolePermissionDataVM> userRolePermissionsList;

            bool isExist = _memoryCache.TryGetValue(loggedUserId, out userRolePermissionsList);

            return userRolePermissionsList;
        }

        public bool IsAllowed(Task<AuthenticationState> authenticationState, PermissionType permissionType, string moduleName)
        {
            List<UserRolePermissionDataVM> userRolePermissionsList = GetAsync(authenticationState).Result;

            bool isAllowed = userRolePermissionsList.Where(p => p.IsAllowed == true &&
                              p.ModuleName.ToLower() == moduleName.ToLower() && p.PermissionType == permissionType.ToString()).Count() > 0;
            
            return isAllowed;
        }

        public async Task<bool> IsSuperAdmin(Task<AuthenticationState> authenticationState)
        {
            var cp = (await authenticationState).User;

            string claimValue = cp.Claims.Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value).SingleOrDefault();

            return Convert.ToInt32(claimValue) == ((int)UserRole.SuperAdmin);
        }
    }
}
