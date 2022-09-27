using DataModels.Enums;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using System.Text;
using Telerik.Blazor.Components;
using Web.UI.Extensions;
using Web.UI.Utilities;

namespace Web.UI.Pages.Aircraft
{
    public partial class Index
    {
        #region Params
        [CascadingParameter] public TelerikListView<AircraftDataVM> dataListView { get; set; }
        [CascadingParameter] public TelerikGrid<AircraftDataVM> dataGridView { get; set; }
        [Parameter] public int? ParentCompanyId { get; set; }
        [Parameter] public long? UserId { get; set; }
        [Parameter] public string ParentModuleName { get; set; }

        #endregion

        AircraftFilterVM aircraftFilterVM;
        List<AircraftDataVM> airCraftsVM;
        AircraftVM aircraftData;

        string moduleName = "Aircraft";
        bool isDisplayGridView = true;
        int companyId; int listViewPageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;

        protected override async Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);

            if (ParentCompanyId != null)
            {
                companyId = ParentCompanyId.Value;
            }

            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            if (!_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            SetSelectedMenuItem(moduleName);

            aircraftFilterVM = new AircraftFilterVM();
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            aircraftFilterVM = await AircraftService.GetFiltersAsync(dependecyParams);

            ChangeLoaderVisibilityAction(false);
        }

        private void OnCompanyValueChanges(int selectedValue)
        {
            if (aircraftFilterVM.CompanyId != selectedValue)
            {
                RefreshGrid();
                aircraftFilterVM.CompanyId = selectedValue;
            }
        }

        async Task LoadListViewData(ListViewReadEventArgs args)
        {
            AircraftDatatableParams datatableParams = new DatatableParams().Create(args, "TailNo").Cast<AircraftDatatableParams>();
            await LoadData(datatableParams);

            listViewPageSize = datatableParams.Length;
            args.Total = airCraftsVM.Count() > 0 ? airCraftsVM[0].TotalRecords : 0;
            args.Data = airCraftsVM;
        }

        async Task LoadData(GridReadEventArgs args)
        {
            AircraftDatatableParams datatableParams = new DatatableParams().Create(args, "TailNo").Cast<AircraftDatatableParams>();
            await LoadData(datatableParams);

            pageSize = datatableParams.Length;
            args.Total = airCraftsVM.Count() > 0 ? airCraftsVM[0].TotalRecords : 0;
            args.Data = airCraftsVM;
        }

        public void ListViewPageSizeChangedHandler(int newPageSize)
        {
            listViewPageSize = newPageSize;
        }

        private async Task LoadData(AircraftDatatableParams datatableParams)
        {
            datatableParams.CompanyId = companyId;
            datatableParams.SearchText = searchText;
            datatableParams.IsActive = true;
            pageSize = datatableParams.Length;

            if(pageSize == 0)
            {
                pageSize = 10;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            airCraftsVM = await AircraftService.ListAsync(dependecyParams, datatableParams);
        }

        void RefreshGrid()
        {
            if (dataGridView != null)
            {
                dataGridView.Rebind();
            }

            if (dataListView != null)
            {
                dataListView.Rebind();
            }
        }

        async Task AircraftCreateDialog(AircraftDataVM aircraftVM)
        {
            if (aircraftVM.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Create Aircraft";
            }
            else
            {
                operationType = OperationType.Edit;
                aircraftVM.IsLoadingEditButton = true;
                popupTitle = "Update Aircraft Details";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            aircraftData = await AircraftService.GetDetailsAsync(dependecyParams, aircraftVM.Id);

            if (aircraftData.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                aircraftVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
        }

        async Task OpenDetailPage(long aircraftId)
        {
            if (_currentUserPermissionManager.IsAllowed(AuthStat, PermissionType.Edit, moduleName))
            {
                byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(aircraftId.ToString() + "FlyManager");
                var data = Encoding.Default.GetBytes(aircraftId.ToString());
                NavigationManager.NavigateTo("AircraftDetails?AircraftId=" + System.Convert.ToBase64String(encodedBytes));
            }
        }

        async Task DeleteAsync(long id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftService.DeleteAsync(dependecyParams, id);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        void CloseDialog(bool reloadGrd)
        {
            isDisplayPopup = false;

            if (reloadGrd)
            {
                RefreshGrid();
            }
        }

        void ChangeView(bool isGridView)
        {
            isDisplayGridView = isGridView;

        }

        async Task OpenDeleteDialog(AircraftDataVM aircraftDetails)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Aircraft";

            aircraftData = new AircraftVM();
            aircraftData.Id = aircraftDetails.Id;
            aircraftData.TailNo = aircraftDetails.TailNo;
        }
    }
}
