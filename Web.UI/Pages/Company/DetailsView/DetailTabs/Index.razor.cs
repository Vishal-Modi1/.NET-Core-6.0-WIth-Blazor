using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

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
    }
}
