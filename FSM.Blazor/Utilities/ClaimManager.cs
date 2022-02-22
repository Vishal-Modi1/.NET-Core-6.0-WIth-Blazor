using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FSM.Blazor.Utilities
{
    public static class ClaimManager
    {
        public static string GetClaimValue(AuthenticationStateProvider authenticationStateProvider, string claimType)
        {
            ClaimsPrincipal cp = authenticationStateProvider.GetAuthenticationStateAsync().Result.User;

            string claimValue = cp.Claims.Where(c => c.Type == claimType)
                               .Select(c => c.Value).SingleOrDefault();

            return claimValue;
        }
    }
}
