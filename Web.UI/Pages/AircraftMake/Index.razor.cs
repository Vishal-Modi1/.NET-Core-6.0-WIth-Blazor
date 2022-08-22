using DataModels.VM.Common;
using Web.UI.Data.AircraftMake;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.AircraftMake;
using DataModels.Enums;
using DE = DataModels.Entities;
using Web.UI.Extensions;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.AircraftMake
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<AircraftMakeDataVM> grid { get; set; }

        IList<AircraftMakeDataVM> data;
        string moduleName = Module.Aircraft.ToString();

        bool isDisplayPopup { get; set; }
        string popupTitle { get; set; }

        DE.AircraftMake _aircraftMake { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            DatatableParams datatableParams = new DatatableParams().Create(args, "Name");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;
            
            data = await AircraftMakeService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        async Task AircraftMakeCreateDialog(AircraftMakeDataVM aircraftMake)
        {
            _aircraftMake = new DE.AircraftMake();
            _aircraftMake.Id = aircraftMake.Id;
            _aircraftMake.Name = aircraftMake.Name;

            if (_aircraftMake.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Add Make";
            }
            else
            {
                operationType = OperationType.Edit;
                popupTitle = "Edit Make";
                aircraftMake.IsLoadingEditButton = true;
            }

            isDisplayPopup = true;

            if (_aircraftMake.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                aircraftMake.IsLoadingEditButton = false;
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

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await AircraftMakeService.DeleteAsync(dependecyParams, id);

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                await CloseDialog(true);
            }
            else
            {
                await CloseDialog(false);
            }
        }

        async Task OpenDeleteDialog(AircraftMakeDataVM aircraftMake)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _aircraftMake = new DE.AircraftMake();

            _aircraftMake.Id = aircraftMake.Id;
            _aircraftMake.Name = aircraftMake.Name;

            popupTitle = "Delete Make";
        }
    }
}
