using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
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
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
        }

        public async Task<List<MenuItem>> ListMenuItemsAsync(Task<AuthenticationState> authenticationState, AuthenticationStateProvider authenticationStateProvider)
        {
            List<UserRolePermissionDataVM> userRolePermissionsList = await _currentUserPermissionManager.GetAsync(authenticationState);

            if (userRolePermissionsList == null || userRolePermissionsList.Count() == 0)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, $"UserRolePermission/listbyroleid", "", authenticationStateProvider);
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                if (response != null)
                {
                    userRolePermissionsList = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data.ToString());

                    if (userRolePermissionsList != null && userRolePermissionsList.Count() > 0)
                    {
                        var cp = (await authenticationState).User;

                        string claimValue = cp.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                                   .Select(c => c.Value).SingleOrDefault();

                        _currentUserPermissionManager.AddInCache(Convert.ToInt32(claimValue), userRolePermissionsList);
                    }
                }
            }

            if(userRolePermissionsList == null)
            {
                return new List<MenuItem>();
            }

            userRolePermissionsList = userRolePermissionsList.Where(p => p.IsAllowed == true && p.PermissionType == PermissionType.View.ToString()).ToList();

            List<MenuItem> menuItemsList = new List<MenuItem>();

            foreach (UserRolePermissionDataVM userRolePermission in userRolePermissionsList)
            {
                MenuItem menuItem = new MenuItem();

                menuItem.Action = userRolePermission.ActionName;
                menuItem.Name = userRolePermission.ModuleName;
                menuItem.Controller = userRolePermission.ControllerName;
                menuItem.DisplayName = userRolePermission.DisplayName;
                menuItem.IsAdministrationModule = userRolePermission.IsAdministrationModule;
                menuItem.FavIconStyle = userRolePermission.Icon;

                menuItemsList.Add(menuItem);
            }

            return menuItemsList;
        }
    }
}
