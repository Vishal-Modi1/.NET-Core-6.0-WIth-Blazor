using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DataModels.VM.UserRolePermission;
using DataModels.Enums;
using FSM.Blazor.Utilities;
using DataModels.Constants;

namespace FSM.Blazor.Filters
{
    public class CustomAuthorization : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string loggedInUserId = context.HttpContext.User.Claims.Where(c => c.Type == CustomClaimTypes.UserId)
                              .Select(c => c.Value).SingleOrDefault();

            if(string.IsNullOrWhiteSpace(loggedInUserId))
            {
                return;
            }

            List<UserRolePermissionDataVM> userPermissionList = CurrentUserPermissionManager.GetAsync().Result;

            string controllerName = (string)context.RouteData.Values["Controller"];
            string actionName = (string)context.RouteData.Values["Action"];

            userPermissionList = userPermissionList.Where(p => p.ModuleName.ToLower() == controllerName.ToLower() 
                                && p.IsAllowed == true ).ToList();

            if (userPermissionList.Count() == 0)
            {
                SetErrorRoute(context);
                return;
            }

            if (actionName == "Index" && !CurrentUserPermissionManager.IsAllowed(PermissionType.View, controllerName.ToLower()))
            {
                SetErrorRoute(context);
                return;
            }

            if (actionName == "Create" && !CurrentUserPermissionManager.IsAllowed(PermissionType.Create, controllerName.ToLower()) && context.HttpContext.Request.Method == "GET" )
            {
                SetErrorRoute(context);
                return;
            }

            if (actionName == "Delete" && !CurrentUserPermissionManager.IsAllowed(PermissionType.Delete, controllerName.ToLower()))
            {
                SetErrorRoute(context);
                return;
            }

            if (actionName == "Edit" && !CurrentUserPermissionManager.IsAllowed(PermissionType.Edit, controllerName.ToLower()))
            {
                SetErrorRoute(context);
                return;
            }
        }

        private void SetErrorRoute(AuthorizationFilterContext context)
        {
            context.Result = new RedirectToRouteResult
            (
            new RouteValueDictionary(new
            {
                action = "Index",
                controller = "Home"
            }));
        }
    }
}
