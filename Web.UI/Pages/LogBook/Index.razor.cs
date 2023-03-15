using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Web.UI.Models.Constants;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook
{
    partial class Index
    {
        int cureActiveTabIndex;

        public string LogBookId { get; set; }

        public List<LogBookSummaryVM> logBookSummaries { get; set; } = new();
        public LogBookVM logBookVM { get; set; } = new();
        public List<DropDownLargeValues> AircraftsList { get; set; } = new();
        public List<DropDownSmallValues> RolesList { get; set; } = new();
        public List<DropDownLargeValues> PassengersList { get; set; } = new();
        public List<DropDownLargeValues> UsersList { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                await LoadData();

                StringValues link;
                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                QueryHelpers.ParseQuery(uri.Query).TryGetValue("LogBookId", out link);

                if (link.Count() > 0 && link[0] != "")
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(link[0]);
                    LogBookId = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Replace(UpflyteConstant.QuesryString, "");

                    if (!string.IsNullOrWhiteSpace(LogBookId))
                    {
                        await EditLogBook(Convert.ToInt64(LogBookId));
                    }
                }
            }
        }

        public async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

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

        async Task EditLogBook(long id)
        {
            if (cureActiveTabIndex != 0)
            {
                cureActiveTabIndex = 0;
            }

            ChangeLoaderVisibilityAction(true);

            logBookVM = await LogBookService.GetDetails(dependecyParams, id);

            ChangeLoaderVisibilityAction(false);
        }
    }
}
