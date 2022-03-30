using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace FSM.Blazor.Pages.SubscriptionPlan
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
        public RadzenDataGrid<SubscriptionPlanDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        IList<SubscriptionPlanDataVM> data;
        int count;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        bool isLoading, isBusyAddNewButton, isBusyUpdateStatusButton;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string searchText ="";
        string moduleName = "SubscriptionPlan";
        

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            await LoadData();
        }

        //async Task LoadData(LoadDataArgs args)
        //{
        //    isLoading = true;

        //    DatatableParams datatableParams = new DatatableParams().Create(args, "Name");
        //    pageSize = datatableParams.Length;
        //    datatableParams.SearchText = searchText;

        //    data = await SubscriptionPlanService.ListAsync(_httpClient, datatableParams);
        //    count = data.Count() > 0 ? data[0].TotalRecords : 0;
        //    isLoading = false;
        //}

        async Task LoadData()
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams();
            pageSize = 10;
            datatableParams.SearchText = searchText;
            datatableParams.SortOrderColumn = "Name";
            datatableParams.Length = 10;
            datatableParams.Start = 1;
            datatableParams.OrderType = "ASC";

            data = await SubscriptionPlanService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task ClosePplanStatusUpdateDialogAsync()
        {
            DialogService.Close(false);
            await grid.Reload();
        }

        async Task UpdateIsPlanActiveAsync(bool value, int id)
        {
            SetUpdateStatusButtonState(true);

            CurrentResponse response = await SubscriptionPlanService.UpdateStatus(_httpClient, id, value);

            SetUpdateStatusButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success,  response.Message, "");
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, response.Message, "");
                NotificationService.Notify(message);
            }

            // await grid.Reload();
            await LoadData();
        }

        async Task SubscriptionPlanCreateDialog(int id, string title, bool isCreate)
        {
            if (isCreate)
            {
                SetAddNewButtonState(true);
            }
            else
            {
                SetEditButtonState(id, true);
            }

            SubscriptionPlanVM subscriptionPlanVM = await SubscriptionPlanService.GetDetailsAsync(_httpClient, id);
            subscriptionPlanVM.ModulesList = await ModuleDetailsService.ListDropDownValues(_httpClient);

            if (isCreate)
            {
                SetAddNewButtonState(false);
            }
            else
            {
                SetEditButtonState(id, false);
            }

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "subscriptionPlanData", subscriptionPlanVM } },
                  new DialogOptions() { Width = "500px", Height = "500px" });

            // await grid.Reload();
            await LoadData();

        }

        async Task DeleteAsync(int id)
        {
            CurrentResponse response = await SubscriptionPlanService.DeleteAsync(_httpClient, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Subscription Plan Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Subscription Plan Details", response.Message);
                NotificationService.Notify(message);
            }

            // await grid.Reload();
            await LoadData();
        }

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }

        private void SetEditButtonState(int id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();

            details.IsLoadingEditButton = isBusy;

            StateHasChanged();
        }

        private void SetUpdateStatusButtonState(bool isBusy)
        {
            isBusyUpdateStatusButton = isBusy;
            StateHasChanged();
        }
    }
}
