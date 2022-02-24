using DataModels.VM.Common;
using DataModels.VM.Reservation;
using FSM.Blazor.Data.Reservation;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using FSM.Blazor.Pages.Scheduler;
using DataModels.VM.Scheduler;
using DataModels.Enums;
using DataModels.Constants;
using Utilities;

namespace FSM.Blazor.Pages.Reservation
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }


        [CascadingParameter]
        public RadzenDataGrid<ReservationDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        ReservationDataTableParams datatableParams;

        ReservationFilterVM reservationFilterVM = new ReservationFilterVM();
        IList<DropDownValues> CompanyFilterDropdown;

        SchedulerVM schedulerVM;

        string timezone = "";
        private bool isDisplayLoader;

        #region Filters
        public int CompanyId;
        public DateTime? startDate, endDate;
        IList<ReservationDataVM> data;
        int count;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        bool isLoading;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string searchText;

        #endregion

        string moduleName = "Reservation";

        UIOptions uiOptions = new UIOptions();

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            timezone = ClaimManager.GetClaimValue(authenticationStateProvider, CustomClaimTypes.TimeZone);
            reservationFilterVM = await ReservationService.GetFiltersAsync(_httpClient);
        }

        async void OnStartDateChange(DateTime? value)
        {
            datatableParams.StartDate = startDate = value;
            await LoadDataAsync();
        }

        async void OnEndDateChange(DateTime? value)
        {
            datatableParams.EndDate = endDate = value;
            await LoadDataAsync();
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            datatableParams = new ReservationDataTableParams().Create(args, "StartDateTime");
            pageSize = datatableParams.Length;
            datatableParams.SearchText = searchText;
            datatableParams.StartDate = startDate;
            datatableParams.EndDate = endDate;
            datatableParams.CompanyId = reservationFilterVM.CompanyId;

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            if (datatableParams.StartDate != null)
            {
                datatableParams.StartDate = DateConverter.ToUTC(datatableParams.StartDate.Value.Date, timezone);
            }

            if (datatableParams.EndDate != null)
            {
                datatableParams.EndDate = DateConverter.ToUTC(datatableParams.EndDate.Value.Date.AddDays(1).AddTicks(-1), timezone);
            }

            data = await ReservationService.ListAsync(_httpClient, datatableParams);

            data.ToList().ForEach(p =>
            {
                p.StartDateTime = DateConverter.ToLocal(p.StartDateTime, timezone);
                p.EndDateTime = DateConverter.ToLocal(p.EndDateTime, timezone);
            });

            if (datatableParams.StartDate != null)
            {
                datatableParams.StartDate = DateConverter.ToLocal(datatableParams.StartDate.Value, timezone);
            }

            if (datatableParams.EndDate != null)
            {
                datatableParams.EndDate = DateConverter.ToLocal(datatableParams.EndDate.Value, timezone);
            }

            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;

            base.StateHasChanged();
        }

        async Task OpenSchedulerDialog(long id)
        {
            isDisplayLoader = true;

            InitializeValues();

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(_httpClient, id);

            schedulerVM.StartTime = DateConverter.ToLocal(schedulerVM.StartTime, timezone);
            schedulerVM.EndTime = DateConverter.ToLocal(schedulerVM.EndTime, timezone);

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime = DateConverter.ToLocal(schedulerVM.AircraftSchedulerDetailsVM.CheckOutTime.Value, timezone);
            }

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime != null)
            {
                schedulerVM.AircraftSchedulerDetailsVM.CheckInTime = DateConverter.ToLocal(schedulerVM.AircraftSchedulerDetailsVM.CheckInTime.Value, timezone);
            }

            uiOptions.dialogVisibility = true;

            uiOptions.isDisplayForm = false;
            uiOptions.isDisplayCheckOutOption = false;

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime == null)
            {
                uiOptions.isDisplayCheckOutOption = true;
            }

            uiOptions.isDisplayMainForm = true;
            uiOptions.isDisplayCheckInButton = schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut;

            isDisplayLoader = false;
        }

        public async Task RefreshSchedulerDataSourceAsync(ScheduleOperations scheduleOperations)
        {
            await LoadDataAsync();
        }

        private void CloseDialog()
        {
            uiOptions.dialogVisibility = false;
            base.StateHasChanged();
        }

        private void OpenDialog()
        {
            uiOptions.dialogVisibility = true;
            base.StateHasChanged();
        }

        public async Task DeleteEventAsync()
        {
            await LoadDataAsync();
        }

        public void InitializeValues()
        {
            uiOptions.isDisplayRecurring = true;
            uiOptions.isDisplayMember1Dropdown = true;
            uiOptions.isDisplayAircraftDropDown = true;
            uiOptions.isDisplayMember2Dropdown = false;
            uiOptions.isDisplayFlightRoutes = false;
            uiOptions.isDisplayInstructor = false;
            uiOptions.isDisplayFlightInfo = false;
            uiOptions.isDisplayStandBy = true;
            uiOptions.isDisplayForm = true;
            uiOptions.isDisplayCheckOutOption = false;
            uiOptions.isDisplayMainForm = true;
        }
    }
}
