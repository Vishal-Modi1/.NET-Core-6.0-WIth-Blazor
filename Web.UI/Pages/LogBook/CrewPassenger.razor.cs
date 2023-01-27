using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;

namespace Web.UI.Pages.LogBook
{
    partial class CrewPassenger
    {
        [Parameter] public LogBookCrewPassengerVM LogBookCrewPassengerVM { get; set; }
        [Parameter] public List<DropDownLargeValues> CrewPassengersList { get; set; }
    }
}
