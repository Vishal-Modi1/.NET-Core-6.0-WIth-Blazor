﻿using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.Common;
using FSM.Blazor.Data.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FSM.Blazor.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter]  public bool Expanded { get; set; }

        bool sidebarExpanded = true, bodyExpanded = false, isSuperAdmin;

        string fullName = "", profileImageURL = "";
        
        IEnumerable<MenuItem> menuItems;

        protected override async Task OnInitializedAsync()
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null &&
                 httpContextAccessor.HttpContext.Request != null && httpContextAccessor.HttpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                var userAgent = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
                if (!string.IsNullOrEmpty(userAgent))
                {
                    if (userAgent.Contains("iPhone") || userAgent.Contains("Android") || userAgent.Contains("Googlebot"))
                    {
                        sidebarExpanded = false;
                        bodyExpanded = true;
                    }
                }
            }

            var cp = (await AuthStat).User;

            if (cp.Identity.IsAuthenticated)
            {
                menuItems = await MenuService.ListMenuItemsAsync(AuthStat, AuthenticationStateProvider);

                fullName = cp.Claims.Where(c => c.Type == CustomClaimTypes.FullName)
                          .Select(c => c.Value).SingleOrDefault();

                profileImageURL = cp.Claims.Where(c => c.Type == CustomClaimTypes.ProfileImageURL)
                           .Select(c => c.Value).SingleOrDefault();

                isSuperAdmin = Convert.ToUInt32(cp.Claims.Where(c => c.Type == ClaimTypes.Role).First().Value) == (int)UserRole.SuperAdmin;
            }
        }
    }
}
