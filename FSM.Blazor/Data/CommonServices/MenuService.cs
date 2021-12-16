using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace FSM.Blazor.Data.CommonServices
{
    public class MenuService
    {
        private List<MenuItem> menuItems;

        private AuthenticationStateProvider _authenticationStateProvider;

        public MenuService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        private HttpCaller _httpCaller = new HttpCaller();

        public async Task<List<MenuItem>> ListMenuItemsAsync(IHttpClientFactory _httpClient)
        {

            List<UserRolePermissionDataVM> userRolePermissionsList = CurrentUserPermissionManager.GetAsync().Result;


            if (userRolePermissionsList == null || userRolePermissionsList.Count == 0)
            {
                string apiURL = @"https://localhost:7132/api/UserRolePermission/listbyroleid";

                var request = new HttpRequestMessage(HttpMethod.Get, apiURL);
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetClaimValue(CustomClaimTypes.AccessToken));

                var client = _httpClient.CreateClient();

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                CurrentResponse response = JsonConvert.DeserializeObject<CurrentResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);
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

        public string GetClaimValue(string claimType)
        {
            ClaimsPrincipal cp = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();

            return claimValue;
        }
    }
}
