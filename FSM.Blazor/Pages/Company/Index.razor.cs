using DataModels.VM.Common;
using DataModels.VM.Company;
using FSM.Blazor.Data.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using DataModels.Enums;

namespace FSM.Blazor.Pages.Company
{
    partial class Index
    {
        #region Params

        [Inject] IHttpClientFactory _httpClient { get; set; }
        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        [Inject] protected IMemoryCache memoryCache { get; set; }
        [CascadingParameter] public RadzenDataGrid<CompanyVM> grid { get; set; }
        [Inject] NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<CompanyVM> data;
        int count;
        bool isLoading;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = Module.Company.ToString();

        bool IsDisplayPopup { get; set; }
        string OpenPopupTitle { get; set; }
        
        OperationType operationType = OperationType.Create;
        CompanyVM _companyData { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            data = await CompanyService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task CompanyCreateDialog(CompanyVM companyData, string title)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            _companyData = companyData;
            OpenPopupTitle = title;
            IsDisplayPopup = true;

            if (companyData.Id == 0) {

                operationType = OperationType.Create;
            }
            else {
                operationType = OperationType.Edit;

            }
            //await DialogService.OpenAsync<Create>(title,
            //      new Dictionary<string, object>() { { "companyData", companyData } },
            //      new DialogOptions() { Width = "550px", Height = "620px" });

            //await grid.Reload();
        }

        async Task CloseDiloag(bool isCancelled)
        {
            if(!isCancelled)
            {
                grid.Reload();
            }

            IsDisplayPopup = false;
        }

        async Task OpenCompanyDetailPage(CompanyVM companyData)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            if (_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Edit, moduleName))
            {
                byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(companyData.Id.ToString() + "FlyManager");
                var data = Encoding.Default.GetBytes(companyData.Id.ToString());
                NavigationManager.NavigateTo("CompanyDetails?CompanyId=" + System.Convert.ToBase64String(encodedBytes));
            }
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                //DialogService.Close(true);
                IsDisplayPopup = false;
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Company Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Company Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async Task OpenDeleteDialog(CompanyVM companyVM)
        {
            IsDisplayPopup = true;
            operationType = OperationType.Delete;
            _companyData = companyVM;
            OpenPopupTitle = "Delete Company";
        }
    }
}
