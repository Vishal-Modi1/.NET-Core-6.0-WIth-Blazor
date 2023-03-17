using DataModels.VM.Aircraft;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Web.UI.Models.Constants;
using DataModels.Enums;

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

        public void EditLogBookInfo(long id)
        {
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + UpflyteConstant.QuesryString);
            globalMembers.SelectedItem = globalMembers.MenuItems.Where(p=>p.DisplayName.ToLower() == Module.LogBook.ToString()).First();
            NavigationManager.NavigateTo("LogBook?LogBookId=" + System.Convert.ToBase64String(encodedBytes));
        }
    }
}
