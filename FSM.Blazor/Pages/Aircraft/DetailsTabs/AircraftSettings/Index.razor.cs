using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs.AircraftSettings
{
    partial class Index
    {
        [Parameter] public long AircraftId { get; set; }

        [Parameter] public long CreatedBy { get; set; }
    }
}
