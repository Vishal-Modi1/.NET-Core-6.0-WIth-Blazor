using FSM.Blazor.Pages.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.Net;

namespace FSM.Blazor.Extensions
{
    public class CustomErrorBoundry : ErrorBoundary
    {
        [Parameter]
        public NavigationManager navigationManager { get; set; }
        
        [Parameter]
        public DialogService DialogService { get; set; }

        protected override async Task<Task> OnErrorAsync(Exception exception)
        {
            if(exception.Message == HttpStatusCode.Unauthorized.ToString())
            {
                await DialogService.OpenAsync<RefreshToken>("Session Timeout!",
                 new Dictionary<string, object>() { { "userData", ""} },
                 new DialogOptions() { Width = "300px", Height = "200px" });
            }

            return base.OnErrorAsync(exception);
        }
    }
}
