using DataModels.VM.Common;
using DataModels.VM.Reservation;
using Web.UI.Data.Reservation;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using DataModels.VM.Scheduler;
using DataModels.Enums;
using DataModels.Constants;
using Telerik.Blazor.Components;
using Utilities;
using Web.UI.Pages.Scheduler;

namespace Web.UI.Pages.Reservation
{
    partial class Index
    {
        #region Params

        [Parameter] public long UserId { get; set; }
        [Parameter] public long? AircraftId { get; set; }
        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyId { get; set; }

        [CascadingParameter]
        public TelerikGrid<ReservationDataVM> grid { get; set; }
        ReservationDataTableParams datatableParams;
        ReservationFilterVM reservationFilterVM = new ReservationFilterVM();
        SchedulerVM schedulerVM;

        string timezone = "";
        #endregion

        #region Filters

        public DateTime? startDate, endDate;
        IList<ReservationDataVM> data;
        int reservationFilterTypeId;
        DependecyParams dependecyParams;

        #endregion

        string moduleName = "Reservation";
        UIOptions uiOptions = new UIOptions();
        List<DropDownValues> reservationTypeFilter;

        protected override async Task OnInitializedAsync()
        {
            GetReservationTypeFilter();

            reservationFilterVM.Aircrafts = new List<DropDownLargeValues>();
            reservationFilterVM.Companies = new List<DropDownValues>();
            reservationFilterVM.Users = new List<DropDownLargeValues>();

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            timezone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            reservationFilterVM = await ReservationService.GetFiltersAsync(dependecyParams);

            if (CompanyId != 0 || !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.CompanyId).Result))
            {
                reservationFilterVM.CompanyId = CompanyId == null ? Convert.ToInt32(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.CompanyId).Result) : CompanyId.Value;
                reservationFilterVM.Users = await UserService.ListDropDownValuesByCompanyId(dependecyParams, reservationFilterVM.CompanyId);
            }
        }

        private async Task OpenCreateScheduleDialogAsync(DateTime? startTime = null, DateTime? endTime = null, long? aircraftId = null)
        {

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Create, "Scheduler"))
            {
                //await DialogService.OpenAsync<UnAuthorized>("UnAuthorized",
                //  new Dictionary<string, object>() { { "UnAuthorizedMessage", "You are not authorized to create new reservation. Please contact to your administartor" } },
                //  new DialogOptions() { Width = "410px", Height = "165px" });

                return;
            }

            isBusyAddButton = true;
            InitializeValues();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(dependecyParams, 0);

            if (startTime != null)
            {
                schedulerVM.StartTime = startTime.Value;
                schedulerVM.EndTime = endTime.Value;
            }
            else
            {
                schedulerVM.StartTime = DateTime.Now;
                schedulerVM.EndTime = DateTime.Now.AddMinutes(30);
            }

            if (aircraftId != null)
            {
                schedulerVM.AircraftId = aircraftId;
            }

            isBusyAddButton = false;
            isDisplayPopup = true;
            popupTitle = "Schedule Appointment";
        }

        private void GetReservationTypeFilter()
        {
            reservationTypeFilter = new List<DropDownValues>();
            List<ReservationType> reservationFilterList = Enum.GetValues(typeof(ReservationType))
                           .Cast<ReservationType>()
                           .ToList();

            foreach (ReservationType reservationFilter in reservationFilterList)
            {
                reservationTypeFilter.Add(new DropDownValues()
                {
                    Id = ((int)reservationFilter),
                    Name = reservationFilter.ToString()
                });
            }
        }

        async void GetUsersList(int value)
        {
            reservationFilterVM.CompanyId = value;
            reservationFilterVM.Users = await UserService.ListDropDownValuesByCompanyId(dependecyParams, reservationFilterVM.CompanyId);
            ReloadData();
        }

        async void OnStartDateChange()
        {
            datatableParams.StartDate = startDate;
            ReloadData();
        }

        async void OnEndDateChange()
        {
            datatableParams.EndDate = endDate;
            ReloadData();
        }

        async Task LoadData(GridReadEventArgs args)
        {
            datatableParams = new ReservationDataTableParams().Create(args, "StartDateTime").Cast<ReservationDataTableParams>();
            pageSize = datatableParams.Length;
            datatableParams.SearchText = searchText;
            datatableParams.StartDate = startDate;
            datatableParams.EndDate = endDate;

            if (reservationFilterTypeId != 0)
            {
                datatableParams.ReservationType = (ReservationType)reservationFilterTypeId;
            }

            if (ParentModuleName == Module.Company.ToString())
            {
                datatableParams.CompanyId = CompanyId.GetValueOrDefault();
            }
            else
            {
                datatableParams.CompanyId = reservationFilterVM.CompanyId;
            }

            if (reservationFilterVM.UserId > 0)
            {
                datatableParams.UserId = reservationFilterVM.UserId;
            }

            if (AircraftId == null)
            {
                datatableParams.AircraftId = reservationFilterVM.AircraftId;
            }
            else
            {
                datatableParams.AircraftId = AircraftId;
            }

           await LoadDataAsync(args);
        }

        public async Task LoadDataAsync(GridReadEventArgs args)
        {
            if (datatableParams.StartDate != null)
            {
                datatableParams.StartDate = DateConverter.ToUTC(datatableParams.StartDate.Value.Date, timezone);
            }

            if (datatableParams.EndDate != null)
            {
                datatableParams.EndDate = DateConverter.ToUTC(datatableParams.EndDate.Value.Date.AddDays(1).AddTicks(-1), timezone);
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await ReservationService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

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
        }

        async Task OpenSchedulerDialog(ReservationDataVM reservationDataVM)
        {
            reservationDataVM.IsButtonLoading = true;

            InitializeValues();

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            schedulerVM = await AircraftSchedulerService.GetDetailsAsync(dependecyParams, reservationDataVM.Id);

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

            uiOptions.isDisplayForm = false;
            uiOptions.isDisplayCheckOutOption = false;

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime == null)
            {
                uiOptions.isDisplayCheckOutOption = true;
            }

            uiOptions.isDisplayMainForm = true;
            uiOptions.isDisplayCheckInButton = schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut;

            popupTitle = "Schedule Appointment";
            isDisplayPopup = true;
            reservationDataVM.IsButtonLoading = false;
        }

        public void RefreshSchedulerDataSourceAsync(ScheduleOperations scheduleOperations)
        {
            ReloadData();
        }

        public void ReloadData()
        {
            grid.Rebind();
        }

        private void CloseDialog()
        {
            isDisplayPopup = false;
        }

        private void OpenDialog()
        {
            isDisplayPopup = true;
        }

        public async Task DeleteEventAsync()
        {
            ReloadData();
        }

        public void InitializeValues()
        {
            uiOptions.isDisplayRecurring = true;
            uiOptions.isDisplayMember1Dropdown = true;
            uiOptions.isDisplayAircraftDropDown = true;
            if (schedulerVM == null)
            {
                schedulerVM = new SchedulerVM();
                schedulerVM.IsDisplayMember2Dropdown = false;
            }
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
