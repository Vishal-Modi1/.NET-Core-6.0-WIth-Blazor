using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using GlobalUtilities;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook
{
    partial class RecentLogBooks
    {
        [Parameter] public List<LogBookSummaryVM> logBookSummaries { get; set; }
        [Parameter] public EventCallback<long> EditLogBook { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await LoadData();
            }
        }

        public async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            logBookSummaries = await LogBookService.LogBookSummaries(dependecyParams);

            logBookSummaries.ForEach(x =>
            {
                x.Date = DateConverter.ToLocal(x.Date, globalMembers.Timezone);

            });

            ChangeLoaderVisibilityAction(false);
        }

        public async Task EditLogBookInfo(long id)
        {
            await EditLogBook.InvokeAsync(id);
        }
    }
}
