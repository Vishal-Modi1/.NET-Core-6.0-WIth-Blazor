using DataModels.VM.Common;
using DataModels.VM.BillingHistory;
using FSM.Blazor.Data.BillingHistory;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace FSM.Blazor.Pages.BillingHistory
{
    partial class Index
    {
        #region Params
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<BillingHistoryDataVM> grid { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<BillingHistoryDataVM> data;
        int count;
        bool isLoading;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = "BillingHistory";
        
        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            BillingHistoryDatatableParams datatableParams = new BillingHistoryDatatableParams().Create(args, "CreatedOn");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            data = await BillingHistoryService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task OpenViewBillingInfo(BillingHistoryDataVM billingHistoryDataVM)
        {
            await DialogService.OpenAsync<BillDetails>($"Billing Details",
                  new Dictionary<string, object>() { { "BillingData", billingHistoryDataVM } },
                  new DialogOptions() { Width = "500px", Height = "380px" });
        }
    }
}
