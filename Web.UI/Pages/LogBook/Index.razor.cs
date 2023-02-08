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
        public List<DropDownSmallValues> RolesList { get; set; } = new();
        public List<DropDownLargeValues> PassengersList { get; set; } = new();
        public List<DropDownLargeValues> UsersList { get; set; } = new();

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

            ResetModel();
            
            AircraftsList = await AircraftService.ListDropdownValuesByCompanyId(dependecyParams, globalMembers.CompanyId);
            RolesList = await LogBookService.ListPassengersRolesDropdownValues(dependecyParams);
            PassengersList = await LogBookService.ListPassengersDropdownValuesByCompanyId(dependecyParams);
            UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, globalMembers.CompanyId);

            ChangeLoaderVisibilityAction(false);
        }

        async Task RefreshSummaries()
        {
            ChangeLoaderVisibilityAction(true);

            ResetModel();
            logBookSummaries = await LogBookService.LogBookSummaries(dependecyParams);

            ChangeLoaderVisibilityAction(false);
        }

        void ResetModel()
        {
            logBookVM = new LogBookVM();
            logBookVM.Date = DateTime.Now;
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
