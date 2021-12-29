using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Data.Aircraft;
using FSM.Blazor.Extensions;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        public RadzenDataList<AircraftDataVM> grid { get; set; }

        [CascadingParameter]
        public RadzenDropDown<int> companyFilter { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        AircraftFilterVM aircraftFilterVM;
        IList<DropDownValues> CompanyFilterDropdown;

        int CompanyId; 
        bool isLoading;
        string searchText;

        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = 6;
        int count;
        List<AircraftDataVM> airCraftsVM;

        protected override async Task OnInitializedAsync()
        {
            aircraftFilterVM = await AircraftService.GetFiltersAsync(_httpClient);
            CompanyFilterDropdown = aircraftFilterVM.Companies;
        }

        void PageChanged(PagerEventArgs args)
        {
            //airCraftsVM = GetOrders(args.Skip, args.Top);
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            AircraftDatatableParams datatableParams = new AircraftDatatableParams().Create(args, "TailNo");

            datatableParams.CompanyId = CompanyId;
            datatableParams.SearchText = searchText;
            datatableParams.IsActive = true;

            airCraftsVM = await AircraftService.ListAsync(_httpClient, datatableParams);
            count = airCraftsVM.Count() > 0 ? airCraftsVM[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task AircraftCreateDialog(int id, string title)
        {
            AirCraftVM aircraftData = await AircraftService.GetDetailsAsync(_httpClient, id);


            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "aircraftData", aircraftData } },
                  new DialogOptions() { Width = "800px", Height = "580px" });

            await grid.Reload();
        }
    }
}
