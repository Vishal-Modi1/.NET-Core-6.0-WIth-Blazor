using Telerik.Blazor.Components;
using DataModels.VM.LogBook;
using DataModels.VM.Common;
using Web.UI.Extensions;
using Web.UI.Utilities;
using Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.Enums;

namespace Web.UI.Pages.LogBook
{
    partial class LogBookList
    {
        [Parameter] public EventCallback<long> EditLogBook { get; set; }
        [Parameter] public EventCallback RefreshSummaries { get; set; }
        [CascadingParameter] public TelerikGrid<LogBookDataVM> grid { get; set; }
        LogBookDatatableParams datatableParams;
        LogBookFilterVM logBookFilterVM;
        IList<LogBookDataVM> data;
        LogBookDataVM selectedLogBook;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            logBookFilterVM = new LogBookFilterVM();

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            logBookFilterVM = await LogBookService.GetFiltersAsync(dependecyParams);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            datatableParams = new DatatableParams().Create(args, "DisplayName").Cast<LogBookDatatableParams>();

            datatableParams.UserId = logBookFilterVM.UserId;
            datatableParams.AircraftId = logBookFilterVM.AircraftId;
            datatableParams.CompanyId = logBookFilterVM.CompanyId;

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            data = await LogBookService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            data.ToList().ForEach(p =>
            {
                p.Date = DateConverter.ToLocal(p.Date, globalMembers.Timezone);
            });

            isGridDataLoading = false;
        }

        private async void OnCompanyValueChanges(int selectedValue)
        {
            if (logBookFilterVM.CompanyId != selectedValue)
            {
                logBookFilterVM.CompanyId = selectedValue;
                grid.Rebind();
                logBookFilterVM.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, selectedValue);
                logBookFilterVM.AircraftsList = await AircraftService.ListDropdownValuesByCompanyId(dependecyParams, selectedValue);
            }
        }

        private void OnAircraftValueChanges(long selectedValue)
        {
            if (logBookFilterVM.AircraftId != selectedValue)
            {
                logBookFilterVM.AircraftId = selectedValue;
                grid.Rebind();
            }
        }

        private void OnUserValueChanges(long selectedValue)
        {
            if (logBookFilterVM.UserId != selectedValue)
            {
                logBookFilterVM.UserId = selectedValue;
                grid.Rebind();
            }
        }

        void OpenDeleteLogBookDialog(LogBookDataVM logBook)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete LogBook";

            selectedLogBook = logBook;
        }

        async Task DeleteAsync()
        {
            isBusyDeleteButton = true;

            CurrentResponse response = await LogBookService.DeleteAsync(dependecyParams, selectedLogBook.Id);

            isBusyDeleteButton = false;

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                isDisplayPopup = false;
                await RefreshSummaries.InvokeAsync();
            }

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
        }

        public async Task EditLogBookInfo(long id)
        {
            await EditLogBook.InvokeAsync(id);
        }
    }
}
