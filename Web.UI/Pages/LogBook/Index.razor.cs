using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook
{
    partial class Index
    {
        int cureActiveTabIndex;

        public List<LogBookSummaryVM> logBookSummaries { get; set; } = new();
        public LogBookVM logBookVM { get; set; } = new();
        public List<DropDownLargeValues> AircraftsList { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                await LoadData();
            }
        }

        public async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            logBookSummaries = await LogBookService.LogBookSummaries(dependecyParams);

            logBookVM = new LogBookVM();
            AircraftsList = await AircraftService.ListDropdownValuesByCompanyId(dependecyParams, globalMembers.CompanyId);

            ChangeLoaderVisibilityAction(false);
        }

        async Task RefreshSummaries()
        {
            ChangeLoaderVisibilityAction(true);

            logBookSummaries = await LogBookService.LogBookSummaries(dependecyParams);

            ChangeLoaderVisibilityAction(false);
        }

        void TabChangedHandler(int newIndex)
        {
            cureActiveTabIndex = newIndex;
        }

        async Task GetLogBookDetails(long id)
        {
            ChangeLoaderVisibilityAction(true);

            logBookVM = await LogBookService.GetDetails(dependecyParams, id);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
