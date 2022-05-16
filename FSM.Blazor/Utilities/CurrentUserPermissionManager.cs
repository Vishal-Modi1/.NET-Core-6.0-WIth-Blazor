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
        private readonly IMemoryCache _MemoryCache;
        private static CurrentUserPermissionManager _instance = null;
        private static readonly object padlock = new object();

        public static CurrentUserPermissionManager GetInstance(IMemoryCache MemoryCache)
        {

            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new CurrentUserPermissionManager(MemoryCache);
                }

                return _instance;
            }
        }

        private CurrentUserPermissionManager(IMemoryCache MemoryCache)
        {
            _MemoryCache = MemoryCache;
        }

        public void AddInCache(long loggedUserId, List<UserRolePermissionDataVM> userRolePermissionsList)
        {
            _MemoryCache.Set(loggedUserId, userRolePermissionsList);
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

            bool isExist = _MemoryCache.TryGetValue(loggedUserId, out userRolePermissionsList);

            return userRolePermissionsList;
        }

        public bool IsAllowed(Task<AuthenticationState> authenticationState, PermissionType permissionType, string moduleName)
        {
            List<UserRolePermissionDataVM> userRolePermissionsList = GetAsync(authenticationState).Result;

            bool isAllowed = userRolePermissionsList.Where(p => p.IsAllowed == true &&
                              p.ModuleName.ToLower() == moduleName.ToLower() && p.PermissionType == permissionType.ToString()).Count() > 0;
            
            return isAllowed;
        }

        public async Task<bool> IsValidUser(Task<AuthenticationState> authenticationState, UserRole userRole)
        {
            var cp = (await authenticationState).User;

            string claimValue = cp.Claims.Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value).SingleOrDefault();

            return Convert.ToInt32(claimValue) == ((int)userRole);
        }

        public async Task<UserRole> GetRole(Task<AuthenticationState> authenticationState)
        {
            var cp = (await authenticationState).User;

            string claimValue = cp.Claims.Where(c => c.Type == ClaimTypes.Role)
                               .Select(c => c.Value).SingleOrDefault();

            UserRole userRole = (UserRole)Convert.ToInt32(claimValue);

            return userRole;
        }

        public async Task<string> GetClaimValue(Task<AuthenticationState> authenticationState, string claimType)
        {
            var cp = (await authenticationState).User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();


            return claimValue;
        }
    }
}
