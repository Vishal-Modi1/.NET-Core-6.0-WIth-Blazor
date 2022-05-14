using FSM.Blazor.Shared;
using FSM.Blazor.Shared.Components;

namespace FSM.Blazor.Pages.Dashboard
{
    public partial class Index
    {
        public bool ShowPopup { get; set; }
        public FlightCommonPopupModal _demoPopup { get; set; } = new FlightCommonPopupModal();

    }
}
