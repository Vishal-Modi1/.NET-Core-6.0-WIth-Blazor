using DataModels.VM.Common;
using DataModels.VM.Location;
using FSM.Blazor.Data.Location;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using DataModels.Enums;
using Newtonsoft.Json;

namespace FSM.Blazor.Pages.Location
{
    partial class Index
    {
        #region Params
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<LocationDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        #endregion

        IList<LocationDataVM> data;
        int count;
        bool isLoading;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        string moduleName = Module.Location.ToString();

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            //if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            //{
            //    NavigationManager.NavigateTo("/Dashboard");
            //}
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = new DatatableParams().Create(args, "AirportCode");

            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            data = await LocationService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }

        async Task LocationCreateDialog(int id, string title)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);

            LocationVM locationData = new LocationVM();

            CurrentResponse response = await LocationService.GetDetailsAsync(dependecyParams, id);

            if(response != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                locationData = JsonConvert.DeserializeObject<LocationVM>(response.Data.ToString());
                locationData.Timezones = await TimezoneService.ListDropDownValues(dependecyParams);
            }
           
            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "locationData", locationData } },
                  new DialogOptions() { Width = "550px"});

            await grid.Reload();
        }

        async Task DeleteAsync(int id)
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await LocationService.DeleteAsync(dependecyParams, id);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Location Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Location Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }
    }
}
