using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Common
{
    public class MenuService
    {
        private readonly HttpCaller _httpCaller;

        public MenuService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<MenuItem>> ListMenuItemsAsync(IHttpClientFactory _httpClient)
        {
            List<UserRolePermissionDataVM> userRolePermissionsList = CurrentUserPermissionManager.GetAsync().Result;

            if (userRolePermissionsList == null || userRolePermissionsList.Count == 0)
            {
                 CurrentResponse response = await _httpCaller.GetAsync(_httpClient, "UserRolePermission/listbyroleid");


                if(response == null || response.Status != System.Net.HttpStatusCode.OK)
                {
                    return new List<MenuItem>();
                }

                userRolePermissionsList = JsonConvert.DeserializeObject<List<UserRolePermissionDataVM>>(response.Data);
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
