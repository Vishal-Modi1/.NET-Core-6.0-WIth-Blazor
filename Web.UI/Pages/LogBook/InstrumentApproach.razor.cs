using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;

namespace Web.UI.Pages.LogBook
{
    partial class InstrumentApproach
    {
        [Parameter] public LogBookInstrumentApproachVM LogBookInstrumentApproachVM { get; set; }
        [Parameter] public List<DropDownSmallValues> Approaches { get; set; }
    }
}
