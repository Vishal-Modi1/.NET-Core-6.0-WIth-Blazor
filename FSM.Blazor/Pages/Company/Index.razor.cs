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

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }
        [CascadingParameter] public RadzenDataGrid<CompanyVM> grid { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<CompanyVM> data;
        int count;
        bool isLoading, isBusyAddButton, isBusyEditButton;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = Module.Company.ToString();

        bool isDisplayPopup { get; set; }
        string popupTitle { get; set; }
        
        OperationType operationType = OperationType.Create;
        CompanyVM _companyData { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await CompanyService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task CompanyCreateDialog(CompanyVM companyData, string title)
        {
            if (companyData.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
            }
            else
            {
                operationType = OperationType.Edit;
                SetEditButtonState(companyData.Id, true);
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            _companyData = companyData;
            popupTitle = title;
            isDisplayPopup = true;
            
            if (companyData.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                SetEditButtonState(companyData.Id, false);
            }
        }

        private void SetEditButtonState(int id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            details.IsLoadingEditButton = isBusy;
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if(!isCancelled)
            {
                await grid.Reload();
            }
        }

        async Task OpenCompanyDetailPage(CompanyVM companyData)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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
                isDisplayPopup = false;
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
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _companyData = companyVM;
            popupTitle = "Delete Company";
        }
    }
}
