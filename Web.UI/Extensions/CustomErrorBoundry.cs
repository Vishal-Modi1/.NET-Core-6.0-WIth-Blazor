// TODO
//using Web.UI.Pages.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net;

namespace Web.UI.Extensions
{
    public class CustomErrorBoundry : ErrorBoundary
    {
        [Parameter]
        public NavigationManager NavigationManager { get; set; }
        
        // TODO
        //[Parameter]
        //public DialogService DialogService { get; set; }

        protected override async Task<Task> OnErrorAsync(Exception exception)
        {
            if(exception.Message == HttpStatusCode.Unauthorized.ToString())
            {
                // TODO
                //await DialogService.OpenAsync<RefreshToken>("Session Timeout!",
                // new Dictionary<string, object>() { { "userData", ""} },
                // new DialogOptions() { Width = "300px", Height = "200px" });
            }

            return base.OnErrorAsync(exception);
        }
    }
}
