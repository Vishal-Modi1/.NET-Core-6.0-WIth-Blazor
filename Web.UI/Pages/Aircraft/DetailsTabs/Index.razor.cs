using DataModels.VM.Aircraft;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Web.UI.Models.Constants;

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

        public async Task EditLogBookInfo(long id)
        {
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + UpflyteConstant.QuesryString);
            NavigationManager.NavigateTo("LogBook?LogBookId=" + System.Convert.ToBase64String(encodedBytes));
        }
    }
}
