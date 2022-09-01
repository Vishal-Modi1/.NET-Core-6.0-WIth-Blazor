using DataModels.VM.Aircraft;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Aircraft.DetailsTabs
{
    partial class Index
    {
        [Parameter] public AircraftVM aircraftData { get; set; }
        public int activeTabIndex { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
        }

        public bool IsTabSelectedFlag(int activeTab)
        {
            return activeTabIndex == activeTab;
        }
    }
}
