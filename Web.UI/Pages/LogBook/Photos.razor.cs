using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.LogBook
{
    partial class Photos
    {
        [Parameter] public EventCallback OnToggle { get; set; }
        [Parameter] public bool ShowBodyPart { get; set; }

        async Task<bool> TriggerToggle()
        {
            await OnToggle.InvokeAsync();
            return true;
        }
    }
}
