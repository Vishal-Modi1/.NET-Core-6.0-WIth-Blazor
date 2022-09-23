using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.User
{
    partial class Index
    {
        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyIdParam { get; set; }
    }
}
