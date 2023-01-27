using Microsoft.AspNetCore.Components;
using DataModels.VM.LogBook;

namespace Web.UI.Pages.LogBook
{
    partial class TimeDetails
    {
        [Parameter] public LogBookFlightTimeDetailVM LogBookTimeDetails { get; set; }
    }
}
