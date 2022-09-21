//using Web.UI.Pages.Account;
using Microsoft.AspNetCore.Components.Web;
using System.Net;

namespace Web.UI.Extensions
{
    public class CustomErrorBoundry : ErrorBoundary
    {
        protected override async Task<Task> OnErrorAsync(Exception exception)
        {
            if(exception.Message == HttpStatusCode.Unauthorized.ToString())
            {
                //Test 
                //await DialogService.OpenAsync<RefreshToken>("Session Timeout!",
                // new Dictionary<string, object>() { { "userData", ""} },
                // new DialogOptions() { Width = "300px", Height = "200px" });
            }

            return base.OnErrorAsync(exception);
        }
    }
}
