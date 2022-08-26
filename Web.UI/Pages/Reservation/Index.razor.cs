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
        bool isSuperAdmin, isAdmin;

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

            isSuperAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, UserRole.SuperAdmin).Result;
            isAdmin = _currentUserPermissionManager.IsValidUser(AuthStat, UserRole.Admin).Result;

            timezone = ClaimManager.GetClaimValue(AuthenticationStateProvider, CustomClaimTypes.TimeZone);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            reservationFilterVM = await ReservationService.GetFiltersAsync(dependecyParams);

            if (CompanyId != 0 || !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.CompanyId).Result))
            {
                reservationFilterVM.CompanyId = CompanyId == null ? Convert.ToInt32(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.CompanyId).Result) : CompanyId.Value;
                reservationFilterVM.Users = await UserService.ListDropDownValuesByCompanyId(dependecyParams, reservationFilterVM.CompanyId);
            }
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
            grid.Rebind();
        }

        async void OnStartDateChange()
        {
            datatableParams.StartDate = startDate;
            grid.Rebind();
        }

        async void OnEndDateChange()
        {
            datatableParams.EndDate = endDate;
            grid.Rebind();
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

            //if (!isSuperAdmin && !isAdmin)
            //{
            //    datatableParams.UserId = UserId;
            //}

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

            uiOptions.dialogVisibility = true;

            uiOptions.isDisplayForm = false;
            uiOptions.isDisplayCheckOutOption = false;

            if (schedulerVM.AircraftSchedulerDetailsVM.CheckInTime == null)
            {
                uiOptions.isDisplayCheckOutOption = true;
            }

            uiOptions.isDisplayMainForm = true;
            uiOptions.isDisplayCheckInButton = schedulerVM.AircraftSchedulerDetailsVM.IsCheckOut;

            isDisplayPopup = true;
            reservationDataVM.IsButtonLoading = false;
        }

        public async Task RefreshSchedulerDataSourceAsync(ScheduleOperations scheduleOperations)
        {
            grid.Rebind();
        }

        public void ReloadData()
        {
            grid.Rebind();
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
          ReloadData();
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
