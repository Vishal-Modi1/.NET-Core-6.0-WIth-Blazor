using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Company.DetailsView.Settings
{
    partial class Index
    {
        [Parameter] public CompanyVM CompanyData { get; set; }
    }
}
