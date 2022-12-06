using DataModels.VM.Common;
using Web.UI.Data.AircraftModel;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.AircraftModel;
using DataModels.Enums;
using DE = DataModels.Entities;
using Web.UI.Extensions;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.AircraftModel
{
    partial class Index
    {
        [CascadingParameter] public TelerikGrid<AircraftModelDataVM> grid { get; set; }
        bool isFilterBarVisible;
        IList<AircraftModelDataVM> data;
        string moduleName = Module.Aircraft.ToString();
        DE.AircraftModel _aircraftModel { get; set; }

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
            
            data = await AircraftModelService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        async Task AircraftModelCreateDialog(AircraftModelDataVM aircraftModel)
        {
            _aircraftModel = new DE.AircraftModel();
            _aircraftModel.Id = aircraftModel.Id;
            _aircraftModel.Name = aircraftModel.Name;

            if (_aircraftModel.Id == 0)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Add Model";
            }
            else
            {
                operationType = OperationType.Edit;
                popupTitle = "Edit Model";
                aircraftModel.IsLoadingEditButton = true;
            }

            isDisplayPopup = true;

            if (_aircraftModel.Id == 0)
            {
                isBusyAddButton = false;
            }
            else
            {
                aircraftModel.IsLoadingEditButton = false;
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
            CurrentResponse response = await AircraftModelService.DeleteAsync(dependecyParams, id);

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

        async Task OpenDeleteDialog(AircraftModelDataVM aircraftModel)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            _aircraftModel = new DE.AircraftModel();

            _aircraftModel.Id = aircraftModel.Id;
            _aircraftModel.Name = aircraftModel.Name;

            popupTitle = "Delete Model";
        }
    }
}
