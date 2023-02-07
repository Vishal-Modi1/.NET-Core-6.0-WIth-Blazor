using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.LogBook
{
    public partial class LogBookLeftPanel
    {
        public bool isDayListVisible;
        [Parameter] public List<LogBookSummaryVM> logBookSummaries { get; set; }
        [Parameter] public EventCallback<long> EditLogBook { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { }
            return base.OnAfterRenderAsync(firstRender);
        }

        
        public async Task EditLogBookInfo(long id)
        {
            await EditLogBook.InvokeAsync(id);
        }
    }
}
