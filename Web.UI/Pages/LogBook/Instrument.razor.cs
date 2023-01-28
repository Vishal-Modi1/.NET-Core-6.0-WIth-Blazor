using Microsoft.AspNetCore.Components;
using DataModels.VM.LogBook;

namespace Web.UI.Pages.LogBook
{
    partial class Instrument
    {
        [Parameter] public LogBookVM logBookVM { get; set; }
        [Parameter] public EventCallback OnToggle { get; set; }
        [Parameter] public bool ShowBodyPart { get; set; }

        void AddNewApproach()
        {
            logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList.Add(new LogBookInstrumentApproachVM());   
        }

        async Task<bool> TriggerToggle()
        {
            await OnToggle.InvokeAsync();
            return true;
        }
    }
}
