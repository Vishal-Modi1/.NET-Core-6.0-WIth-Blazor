using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.LogBook
{
    partial class Create
    {
        [Parameter] public LogBookVM logBookVM { get; set; }
    }
}
