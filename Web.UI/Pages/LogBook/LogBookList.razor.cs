using Telerik.Blazor.Components;
using DataModels.VM.LogBook;
using DataModels.VM.Common;
using Web.UI.Extensions;
using Web.UI.Utilities;
using GlobalUtilities;
using Microsoft.AspNetCore.Components;
using DataModels.Enums;

namespace Web.UI.Pages.LogBook
{
    partial class LogBookList
    {
        #region Params
        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyIdParam { get; set; }
        [Parameter] public long? UserIdParam { get; set; }
        [Parameter] public long? AircraftIdParam { get; set; }
        [Parameter] public bool? IsPersonalDocument { get; set; }

        [Parameter] public EventCallback<long> EditLogBook { get; set; }
        [Parameter] public EventCallback RefreshSummaries { get; set; }
        [CascadingParameter] public TelerikGrid<LogBookDataVM> grid { get; set; }

        #endregion

        LogBookDatatableParams datatableParams;
        LogBookFilterVM logBookFilterVM;
        IList<LogBookDataVM> data;
        LogBookDataVM selectedLogBook;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            logBookFilterVM = new LogBookFilterVM();

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            if (ParentModuleName == Module.Company.ToString())
            {
                await GetDropDownValuesByCompanyId(CompanyIdParam.Value);
            }
            else
            {
                logBookFilterVM = await LogBookService.GetFiltersAsync(dependecyParams);
            }
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            datatableParams = new DatatableParams().Create(args, "DisplayName").Cast<LogBookDatatableParams>();

            SetFilterValues();

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

        private void SetFilterValues()
        {
            if (ParentModuleName == Module.MyProfile.ToString())
            {
                logBookFilterVM.UserId = UserIdParam.Value;
            }

            if (ParentModuleName == Module.Aircraft.ToString())
            {
                logBookFilterVM.AircraftId = AircraftIdParam.Value;
            }

            if (ParentModuleName == Module.Company.ToString())
            {
                logBookFilterVM.CompanyId = CompanyIdParam.Value;
            }

            datatableParams.UserId = logBookFilterVM.UserId;
            datatableParams.AircraftId = logBookFilterVM.AircraftId;
            datatableParams.CompanyId = logBookFilterVM.CompanyId;

            datatableParams.DepartureAirpot = logBookFilterVM.DepartureAirpot;
            datatableParams.ArrivalAirpot = logBookFilterVM.ArrivalAirpot;
        }

        private async void OnCompanyValueChanges(int selectedValue)
        {
            if (logBookFilterVM.CompanyId != selectedValue)
            {
                logBookFilterVM.CompanyId = selectedValue;
                grid.Rebind();

                await GetDropDownValuesByCompanyId(selectedValue);
            }
        }

        private async Task GetDropDownValuesByCompanyId(int companyId)
        {
            logBookFilterVM.AircraftsList = await AircraftService.ListDropdownValuesByCompanyId(dependecyParams, companyId);
            logBookFilterVM.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, companyId);
            logBookFilterVM.DepartureAirpotsList = await LogBookService.ListDepartureAirportsDropDownValuesByCompanyId(dependecyParams, companyId);
            logBookFilterVM.ArrivalAirpotsList = await LogBookService.ListArrivalAirportsDropDownValuesByCompanyId(dependecyParams, companyId);

            base.StateHasChanged();
        }

        private void OnAircraftValueChanges(long selectedValue)
        {
            if (logBookFilterVM.AircraftId != selectedValue)
            {
                logBookFilterVM.AircraftId = selectedValue;
                grid.Rebind();
            }
        }

        private void OnDepartureAirportValueChanges(string selectedValue)
        {
            if (logBookFilterVM.DepartureAirpot != selectedValue)
            {
                logBookFilterVM.DepartureAirpot = selectedValue;
                grid.Rebind();
            }
        }

        private void OnArrivalAirportValueChanges(string selectedValue)
        {
            if (logBookFilterVM.ArrivalAirpot != selectedValue)
            {
                logBookFilterVM.ArrivalAirpot = selectedValue;
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
