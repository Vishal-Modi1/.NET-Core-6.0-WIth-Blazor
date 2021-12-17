using DataModels.VM.Common;
using FSM.Blazor.Data.Common;
using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Shared
{
    public partial class NavMenu
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Parameter]
        public bool Expanded { get; set; }

        bool sidebarExpanded = true;
        bool bodyExpanded = false;

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

            menuItems = await MenuService.ListMenuItemsAsync(_httpClient);
        }

        void FilterPanelMenu(ChangeEventArgs args)
        {
            var term = args.Value.ToString();

          //  menuItems = ExampleService.Filter(term);
        }
    }
}
