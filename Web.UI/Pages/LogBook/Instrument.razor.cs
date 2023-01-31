using Microsoft.AspNetCore.Components;
using DataModels.VM.LogBook;
using DataModels.VM.Common;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook
{
    partial class Instrument
    {
        [Parameter] public LogBookVM logBookVM { get; set; }
        [Parameter] public EventCallback OnToggle { get; set; }
        [Parameter] public bool ShowBodyPart { get; set; }
        public List<DropDownSmallValues> Approaches { get; set; }

        protected override Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                Approaches = await LogBookService.ListInstrumentApproachesDropdownValues(dependecyParams);
                base.StateHasChanged();
            }
        }

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
