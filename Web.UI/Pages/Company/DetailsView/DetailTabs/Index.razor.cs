using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;
using Web.UI.Models.Constants;
using DataModels.Enums;

namespace Web.UI.Pages.Company.DetailsView.DetailTabs
{
    partial class Index
    {
        [Parameter]
        public CompanyVM CompanyData { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
        }

        public void EditLogBookInfo(long id)
        {
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + UpflyteConstant.QuesryString);
            globalMembers.SelectedItem = globalMembers.MenuItems.Where(p => p.DisplayName.ToLower() == Module.LogBook.ToString()).First();
            NavigationManager.NavigateTo("LogBook?LogBookId=" + System.Convert.ToBase64String(encodedBytes));
        }
    }
}
