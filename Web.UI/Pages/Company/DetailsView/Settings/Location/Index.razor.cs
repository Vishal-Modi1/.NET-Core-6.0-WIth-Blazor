using DataModels.VM.Common;
using DataModels.VM.Location;
using Web.UI.Data.Location;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Web.UI.Extensions;
using DataModels.Enums;
using Newtonsoft.Json;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.Company.DetailsView.Settings.Location
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<LocationDataVM> grid { get; set; }
        [Parameter] public int CompanyId { get; set; }
        IList<LocationDataVM> data;
        LocationVM locationData;
        string moduleName = "Location";

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            //if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            //{
            //    NavigationManager.NavigateTo("/Dashboard");
            //}
        }

        async Task LoadData(GridReadEventArgs args)
        {
            DatatableParams datatableParams = new DatatableParams().Create(args, "AirportCode");
            datatableParams.CompanyId = CompanyId;

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await LocationService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        async Task LocationCreateDialog(LocationDataVM locationDetails)
        {
            if (locationDetails.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Add Location";
            }
            else
            {
                operationType = OperationType.Edit;
                popupTitle = "Edit Location";
                locationDetails.IsLoadingEditButton = true;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            locationData = new LocationVM();
            CurrentResponse response = await LocationService.GetDetailsAsync(dependecyParams, locationDetails.Id);

            if(response.Status == System.Net.HttpStatusCode.OK)
            {
                locationData = JsonConvert.DeserializeObject<LocationVM>(response.Data.ToString());
                locationData.Timezones = await TimezoneService.ListDropDownValues(dependecyParams);
            }

            locationData.CompanyId = CompanyId;

            if (locationDetails.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                locationDetails.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LocationService.DeleteAsync(dependecyParams, id);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                await CloseDialog(true);
            }
            else
            {
                await CloseDialog(false);
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

        async Task OpenDeleteDialog(LocationDataVM locationDataVM)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Location";

            locationData = new LocationVM();

            locationData.Id = locationDataVM.Id;
            locationData.AirportCode = locationDataVM.AirportCode;
        }
    }
}
