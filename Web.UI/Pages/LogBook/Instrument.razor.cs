using Microsoft.AspNetCore.Components;
using DataModels.VM.LogBook;

namespace Web.UI.Pages.LogBook
{
    partial class Instrument
    {
        [Parameter] public LogBookVM logBookVM { get; set; }

        void AddNewApproach()
        {
            logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList.Add(new LogBookInstrumentApproachVM());   
        }
    }
}
