using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;

namespace Web.UI.Pages.LogBook.CrewPassenger
{
    partial class CrewPassenger
    {
        [Parameter] public LogBookCrewPassengerVM LogBookCrewPassengerVM { get; set; }
        [Parameter] public List<DropDownLargeValues> CrewPassengersList { get; set; }
        [Parameter] public List<DropDownSmallValues> RolesList { get; set; }
        [Parameter] public List<DropDownLargeValues> PassengersList { get; set; }
        [Parameter] public List<DropDownLargeValues> UsersList { get; set; }
    }
}
