using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;
using Web.UI.Models.Constants;


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

        public async Task EditLogBookInfo(long id)
        {
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + UpflyteConstant.QuesryString);
            NavigationManager.NavigateTo("LogBook?LogBookId=" + System.Convert.ToBase64String(encodedBytes));
        }
    }
}
