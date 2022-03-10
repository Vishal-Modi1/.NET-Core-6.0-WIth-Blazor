using DataModels.VM.Aircraft;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs
{
    partial class Index
    {
        [Parameter]
        public AircraftVM AircraftData { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
        }

    }
}
