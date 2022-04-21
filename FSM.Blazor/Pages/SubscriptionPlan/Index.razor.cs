using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using DataModels.Enums;

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
        bool isLoading, isBusyAddNewButton, isBusyUpdateStatusButton;
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

            SubscriptionDataTableParams datatableParams = new SubscriptionDataTableParams();
            datatableParams.SearchText = searchText;
            datatableParams.SortOrderColumn = "Name";
            datatableParams.Length = 1000;
            datatableParams.Start = 1;
            datatableParams.OrderType = "ASC";

            if(!_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.SuperAdmin).Result)
            {
                datatableParams.IsActive = true;
            }

            data = await SubscriptionPlanService.ListAsync(_httpClient, datatableParams);
            
            isLoading = false;
        }

        async Task ClosePlanStatusUpdateDialogAsync()
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

        async Task BuyNow(int id)
        {
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(id.ToString() + "FlyManager");
            var data = Encoding.Default.GetBytes(id.ToString());
            NavManager.NavigateTo("BuyNow?SubscriptionPlanId=" + System.Convert.ToBase64String(encodedBytes));
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

        async Task BuySubscriptionPlan(int id)
        {

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
