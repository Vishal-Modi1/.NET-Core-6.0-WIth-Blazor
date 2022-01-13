using DataModels.VM.Aircraft;
using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs
{
    partial class Index
    {
        [Parameter]
        public AircraftVM AircraftData { get; set; }
    }
}
