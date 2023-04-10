using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using GlobalUtilities;
using Web.UI.Extensions;
using Web.UI.Utilities;

namespace Web.UI.Pages.Aircraft.DetailsTabs.Discrepancy
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<DiscrepancyDataVM> grid { get; set; }
        [Parameter] public long AircraftIdParam { get; set; }
        [Parameter] public int CompanyIdParam { get; set; }

        DiscrepancyVM discrepancy;

        List<DropDownValues> Statuses { get; set; }
        int statusId = 1;
        DiscrepancyDatatableParams datatableParams;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            Statuses = new List<DropDownValues>();

            Statuses.Add(new DropDownValues() { Id = 1, Name = "Open" });
            Statuses.Add(new DropDownValues() { Id = 2, Name = "Resolved" });
        }

        async Task LoadData(GridReadEventArgs args)
        {
            datatableParams = new DatatableParams().Create(args, "CreatedOn").Cast<DiscrepancyDatatableParams>(); ;

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            datatableParams.AircraftId = AircraftIdParam;
            datatableParams.IsOpen = statusId == 1;

            isGridDataLoading = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            var data = await DiscrepancyService.ListAsync(dependecyParams, datatableParams);

            data.ToList().ForEach(p =>
            {
                p.CreatedOn = DateConverter.ToLocal(p.CreatedOn, globalMembers.Timezone);
            });

            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            isGridDataLoading = false;
        }
        private void OnStatusValueChange(int selectedValue)
        {
            if (statusId != selectedValue && statusId != 0)
            {
                statusId = selectedValue;

                grid.Rebind();
            }
        }

        void CloseDialog(bool reloadGrid)
        {
            if (reloadGrid)
            {
                grid.Rebind();
            }

            isDisplayPopup = false;
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

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            discrepancy = await DiscrepancyService.GetDetailsAsync(dependecyParams, discrepancyDataVM.Id);

            discrepancy.AircraftId = AircraftIdParam;

            if (!globalMembers.IsSuperAdmin)
            {
                discrepancy.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, globalMembers.CompanyId);
                discrepancy.CompanyId = globalMembers.CompanyId;
            }
            else
            {
                discrepancy.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, CompanyIdParam);
                discrepancy.CompanyId = CompanyIdParam;
            }

            discrepancy.StatusList = await DiscrepancyService.ListStatusDropdownValues(dependecyParams);

            if (discrepancyDataVM.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                discrepancyDataVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
            base.StateHasChanged();
        }

        protected async void OnSelect(IEnumerable<DiscrepancyDataVM> data)
        {
            var selectedData = data.FirstOrDefault();

            if (selectedData != null)
            {
                ChangeLoaderVisibilityAction(true);
                await OpenCreateDialog(selectedData);
                ChangeLoaderVisibilityAction(false);
            }
        }
    }
}
