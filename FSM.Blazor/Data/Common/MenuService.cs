using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Common
{
    public class MenuService
    {
        private readonly HttpCaller _httpCaller;
        private readonly CurrentUserPermissionManager _currentUserPermissionManager;
        private readonly IHttpClientFactory _httpClient;

        public MenuService(AuthenticationStateProvider authenticationStateProvider, IHttpClientFactory httpClient, IMemoryCache memoryCache)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
            _httpClient = httpClient;
            _currentUserPermissionManager =  CurrentUserPermissionManager.GetInstance(memoryCache);
        }

        public async Task<List<MenuItem>> ListMenuItemsAsync(Task<AuthenticationState> AuthStat)
        {
            List<UserRolePermissionDataVM> userRolePermissionsList = await _currentUserPermissionManager.GetAsync(AuthStat);

            if(userRolePermissionsList == null || userRolePermissionsList.Count() == 0)
            {
                CurrentResponse response = await _httpCaller.GetAsync(_httpClient, $"UserRolePermission/listbyroleid");

                if (response != null)
                {
                    userRolePermissionsList = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data.ToString());

                    if (userRolePermissionsList != null && userRolePermissionsList.Count() > 0)
                    {
                        var cp = (await AuthStat).User;

                        string claimValue = cp.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                                   .Select(c => c.Value).SingleOrDefault();

                        _currentUserPermissionManager.AddInCache(Convert.ToInt32(claimValue), userRolePermissionsList);
                    }
                }
            }

            userRolePermissionsList = userRolePermissionsList.Where(p => p.IsAllowed == true && p.PermissionType == PermissionType.View.ToString()).ToList();

            List<MenuItem> menuItemsList = new List<MenuItem>();

            foreach (UserRolePermissionDataVM userRolePermission in userRolePermissionsList)
            {
                MenuItem menuItem = new MenuItem();

                menuItem.Action = userRolePermission.ActionName;
                menuItem.Controller = userRolePermission.ControllerName;
                menuItem.DisplayName = userRolePermission.DisplayName;
                menuItem.FavIconStyle = userRolePermission.Icon;

                menuItemsList.Add(menuItem);
            }

            return menuItemsList;
        }
    }
}
