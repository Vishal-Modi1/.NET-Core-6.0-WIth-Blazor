using DataModels.Entities;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using GlobalUtilities;
using Web.UI.Utilities;
using DataModels.VM.Reservation;
using DataModels.VM.Common;

namespace Web.UI.Pages.Scheduler
{
    partial class TabsIndex
    {
        public DateTime currentDate = DateTime.Today;
        SchedulerFilter schedulerFilter = new SchedulerFilter();
        List<SchedulerVM> dataSource;
        List<FlightCategoryVM> flightCategories = new ();
        SchedulerIndex schedulerIndex;
        FlightCategoryVM _flightCategory;
        public List<UpcomingFlight> upcomingFlights = new();
        public List<DropDownValues> companies = new ();
        public int companyId;
        public TelerikTabStrip tabRef { get; set; }
        int cureActiveTabIndex;

        protected override Task OnInitializedAsync()
        {
            isLeftBarVisible = true;
            isFilterBarVisible = true;
            SetSelectedMenuItem("Scheduler");

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                ChangeLoaderVisibilityAction(true);

                if (globalMembers.IsSuperAdmin)
                {
                    companies = await CompanyService.ListDropDownValues(dependecyParams);
                }
                else
                {
                    await LoadCategoris();
                    await LoadUpcomingFlights();
                    await LoadDataAsync();
                }

                ChangeLoaderVisibilityAction(false);
            }
        }

        async Task GetScheduleData(int value)
        {
            companyId = value;
            schedulerFilter.CompanyId = companyId;

            await LoadDataAsync();
            await LoadCategoris();
            await LoadUpcomingFlights();

            await schedulerIndex.GetAutocompleteData(companyId);
        }

        private async Task CheckboxChangedAsync(ChangeEventArgs e, FlightCategoryVM flightCategory)
        {
            flightCategory.IsActive = (bool)e.Value;
            await schedulerIndex.LoadCalendarViewData();
        }

        private async Task LoadUpcomingFlights()
        {
            upcomingFlights = await ReservationService.ListUpcomingFlightsByCompanyId(dependecyParams, companyId);
            upcomingFlights.ForEach(p =>
            {
                p.StartDate = DateConverter.ToLocal(p.StartDate, globalMembers.Timezone);
            });
        }

        async Task LoadCategoris()
        {
            flightCategories = await FlightCategoryService.ListAll(dependecyParams, companyId);
            base.StateHasChanged();
        }

        async Task LoadDataAsync()
        {
            schedulerFilter.StartTime = new DateTime(currentDate.Year, currentDate.Month, 1);
            schedulerFilter.EndTime = schedulerFilter.StartTime.AddMonths(1).AddDays(-1);

            schedulerFilter.StartTime = DateConverter.ToUTC(schedulerFilter.StartTime.Date, globalMembers.Timezone);
            schedulerFilter.EndTime = DateConverter.ToUTC(schedulerFilter.EndTime.Date.AddTicks(-1), globalMembers.Timezone);

            dataSource = await AircraftSchedulerService.ListAsync(dependecyParams, schedulerFilter);

            dataSource.ForEach(x =>
            {
                x.StartTime = DateConverter.ToLocal(x.StartTime, globalMembers.Timezone);
                x.EndTime = DateConverter.ToLocal(x.EndTime, globalMembers.Timezone);
            });
        }

        private async Task OnCalendarDateChanged(DateTime newDate)
        {
            currentDate = newDate;
            await LoadDataAsync();
        }

        void OnCellRenderHandler(CalendarCellRenderEventArgs args)
        {
            if(dataSource != null && dataSource.Any(p=>p.StartTime.Date == args.Date))
            {
                args.Class = "highlightDates";
            }
        }

        void OpenCreateCategoryDialogAsync(FlightCategoryVM flightCategory)
        {
            isDisplayPopup = true;
            _flightCategory = flightCategory;

            if (flightCategory.Id == 0)
            {
                popupTitle = "Create Category";
            }
            else
            {
                popupTitle = "Edit Category";
            }
        }

        void TabChangedHandler(int newIndex)
        {
            cureActiveTabIndex = newIndex;
        }

        async Task CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                await LoadCategoris();
            }
        }
    }
}
