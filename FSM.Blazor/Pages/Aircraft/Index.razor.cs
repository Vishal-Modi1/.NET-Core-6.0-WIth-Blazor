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
        #region Params

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataList<AircraftDataVM> dataListView { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<AircraftDataVM> dataGridView { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        [Parameter]
        public int? ParentCompanyId { get; set; }

        [Parameter]
        public long? UserId { get; set; }

        [Parameter]
        public string ParentModuleName { get; set; }

        #endregion

        AircraftFilterVM aircraftFilterVM;
        IList<DropDownValues> CompanyFilterDropdown;

        private CurrentUserPermissionManager _currentUserPermissionManager;


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
        bool isDisplayGridView = false;
        int companyId;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            aircraftFilterVM = await AircraftService.GetFiltersAsync(dependecyParams);
            CompanyFilterDropdown = aircraftFilterVM.Companies;

            if (ParentCompanyId != null)
            {
                companyId = ParentCompanyId.Value;
            }
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            AircraftDatatableParams datatableParams = new AircraftDatatableParams().Create(args, "TailNo");

            datatableParams.CompanyId = companyId;
            datatableParams.SearchText = searchText;
            datatableParams.IsActive = true;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            airCraftsVM = await AircraftService.ListAsync(dependecyParams, datatableParams);
            count = airCraftsVM.Count() > 0 ? airCraftsVM[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task RefreshGrid()
        {
            await dataGridView.Reload();
            await dataListView.Reload();
        }

        async Task AircraftCreateDialog(long id, string title)
        {
            SetAddNewButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);

            AircraftVM aircraftData = await AircraftService.GetDetailsAsync(dependecyParams, id);

            SetAddNewButtonState(false);

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "aircraftData", aircraftData } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            await dataListView.Reload();
        }

        async Task OpenDetailPage(long aircraftId)
        {
            if (_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Edit, moduleName))
            {
                byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(aircraftId.ToString() + "FlyManager");
                var data = Encoding.Default.GetBytes(aircraftId.ToString());
                NavManager.NavigateTo("AircraftDetails?AircraftId=" + System.Convert.ToBase64String(encodedBytes));
            }
        }

        async Task DeleteAsync(long id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Details", response.Message);
                NotificationService.Notify(message);
            }

            await dataGridView.Reload();
        }

        async Task ChangeView(bool isGridView)
        {
            isDisplayGridView = isGridView;
        }

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }
    }
}
