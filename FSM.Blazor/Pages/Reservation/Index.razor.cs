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

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

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
            data = await ReservationService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;

            base.StateHasChanged();
        }

        async Task OpenSchedulerDialog()
        {

        }
    }
}
