using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using System.Text;
using Telerik.Blazor.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<CompanyVM> grid { get; set; }
        string moduleName = Module.Company.ToString();
        bool isFilterBarVisible;
        CompanyVM _companyData { get; set; }
        string state, city;
        CompanyFilter companyFilter = new CompanyFilter();
        DependecyParams dependecyParams;

        protected override async Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyFilter = await CompanyService.GetFiltersAsync(dependecyParams);

            SetSelectedMenuItem("Company");

            ChangeLoaderVisibilityAction(false);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;
            CompanyDatatableParams datatableParams = new DatatableParams().Create(args, "Name").Cast<CompanyDatatableParams>();

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            datatableParams.City = city;
            datatableParams.State = state;

            var data = await CompanyService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            isGridDataLoading = false;
        }

        async Task CompanyCreateDialog(CompanyVM companyData)
        {
            if (companyData.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Create Company";
            }
            else
            {
                operationType = OperationType.Edit;
                companyData.IsLoadingEditButton = true;
                popupTitle = "Update Company Details";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            _companyData = companyData;
            isDisplayPopup = true;

            if (companyData.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                companyData.IsLoadingEditButton = false;
            }
        }

        async Task CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
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
            isBusyDeleteButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.DeleteAsync(dependecyParams, id);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                await CloseDialog(true);
            }
            else
            {
                await CloseDialog(false);
            }

            isBusyDeleteButton = false;
        }

        void OpenDeleteDialog(CompanyVM companyVM)
        {
            isDisplayPopup = true;

            operationType = OperationType.Delete;
            _companyData = companyVM;
            popupTitle = "Delete Company";
        }
    }
}
