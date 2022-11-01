using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;

namespace Web.UI.Pages.Discrepancy
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<DiscrepancyDataVM> grid { get; set; }
        [Parameter] public long AircraftIdParam { get; set; }

        DiscrepancyVM discrepancy;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            //userFilterVM = await UserService.GetFiltersAsync(dependecyParams);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;
            DiscrepancyDatatableParams datatableParams = new DatatableParams().Create(args, "CreatedOn").Cast<DiscrepancyDatatableParams>(); ;

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            datatableParams.AircraftId = AircraftIdParam;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            var data = await DiscperancyService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            isGridDataLoading = false;
        }

        async Task CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
            }
        }

        async Task OpenCreateDialog(DiscrepancyDataVM discrepancyDataVM)
        {
            if (discrepancyDataVM.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Create New Discrepancy";
            }
            else
            {
                operationType = OperationType.Edit;
                discrepancyDataVM.IsLoadingEditButton = true;
                popupTitle = "Update Discrepancy";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            discrepancy = await DiscperancyService.GetDetailsAsync(dependecyParams, discrepancyDataVM.Id);

            discrepancy.AircraftId = AircraftIdParam;

            if (!globalMembers.IsSuperAdmin)
            {
                discrepancy.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, globalMembers.CompanyId);
                discrepancy.CompanyId = globalMembers.CompanyId;
            }
            else
            {
                discrepancy.CompaniesList = await CompanyService.ListDropDownValues(dependecyParams);
            }

            discrepancy.StatusList = await DiscperancyService.ListStatusDropdownValues(dependecyParams);

            if (discrepancyDataVM.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                discrepancyDataVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
        }

    }
}
