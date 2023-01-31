using Microsoft.AspNetCore.Components;
using DataModels.VM.LogBook;

namespace Web.UI.Pages.LogBook
{
    partial class TrainingDetails
    {
            [Parameter] public LogBookTrainingDetailVM LogBookTrainingDetailVM { get; set; }
            [Parameter] public EventCallback OnToggle { get; set; }
            [Parameter] public bool ShowBodyPart { get; set; }

            async Task<bool> TriggerToggle()
            {
                await OnToggle.InvokeAsync();
                return true;
            }
    }
}
