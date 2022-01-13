using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Data.Aircraft;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Enums;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataList<AircraftDataVM> grid { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        AircraftFilterVM aircraftFilterVM;
        IList<DropDownValues> CompanyFilterDropdown;

        private CurrentUserPermissionManager _currentUserPermissionManager;

        int CompanyId;

        bool isLoading, isBusyAddNewButton;

        #region Grid Variables

        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int count;
        List<AircraftDataVM> airCraftsVM;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;

        #endregion

        string moduleName = "Aircraft";

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            aircraftFilterVM = await AircraftService.GetFiltersAsync(_httpClient);
            CompanyFilterDropdown = aircraftFilterVM.Companies;
        }

        void PageChanged(PagerEventArgs args)
        {
            //airCraftsVM = GetOrders(args.Skip, args.Top);
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            AircraftDatatableParams datatableParams = new AircraftDatatableParams().Create(args, "TailNo");

            datatableParams.CompanyId = CompanyId;
            datatableParams.SearchText = searchText;
            datatableParams.IsActive = true;
            pageSize = datatableParams.Length;

            airCraftsVM = await AircraftService.ListAsync(_httpClient, datatableParams);
            count = airCraftsVM.Count() > 0 ? airCraftsVM[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task AircraftCreateDialog(int id, string title)
        {
            SetAddNewButtonState(true);

            AircraftVM aircraftData = await AircraftService.GetDetailsAsync(_httpClient, id);

            SetAddNewButtonState(false);

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "aircraftData", aircraftData } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            await grid.Reload();
        }

        async Task OpenDetailPage(long aircraftId)
        {
            if (_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Edit, moduleName))
            {
                byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(aircraftId.ToString() + "FlyManager");
                var data  = Encoding.Default.GetBytes(aircraftId.ToString());
                NavManager.NavigateTo("AircraftDetails?AircraftId=" + System.Convert.ToBase64String(encodedBytes));
            }
        }

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }
    }
}
