using DataModels.VM.AircraftEquipment;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using DataModels.VM.Common;

namespace FSM.Blazor.Pages.Aircraft.DetailsTabs.AircraftEquipment
{
    partial class Index
    {
        [Parameter]
        public int AircraftId { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<AircraftEquipmentDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        IList<AircraftEquipmentDataVM> data;
        int count;
        bool isLoading;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            AircraftEquipmentDatatableParams datatableParams = new AircraftEquipmentDatatableParams().Create(args, "Status");
            datatableParams.AircraftId = AircraftId;

            data = await AircraftEquipmentService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task DeleteAsync(int id)
        {
            CurrentResponse response = await AircraftEquipmentService.DeleteAsync(_httpClient, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Aircraft Equipment Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft Equipment Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async void AircraftEquipmentCreateDialog(int id, string title)
        {
            AirCraftEquipmentsVM airCraftEquipmentsVM = await AircraftEquipmentService.GetEquipmentDetailsAsync(_httpClient, id);
            airCraftEquipmentsVM.AirCraftId = AircraftId;

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "airCraftEquipmentsVM", airCraftEquipmentsVM } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            await grid.Reload();
        }
    }
}
