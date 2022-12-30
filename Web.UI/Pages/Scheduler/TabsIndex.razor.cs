using DataModels.Entities;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Utilities;
using Web.UI.Utilities;

namespace Web.UI.Pages.Scheduler
{
    partial class TabsIndex
    {
        public DateTime currentDate = DateTime.Today;
        SchedulerFilter schedulerFilter = new SchedulerFilter();
        List<SchedulerVM> dataSource;
        DependecyParams dependecyParams;
        List<FlightCategory> flightCategories = new ();
        SchedulerIndex schedulerIndex;

        protected override Task OnInitializedAsync()
        {
            isLeftBarVisible = false;
            SetSelectedMenuItem("Scheduler");

            return base.OnInitializedAsync();
        }

        private async Task CheckboxChangedAsync(ChangeEventArgs e, FlightCategory flightCategory)
        {
            flightCategory.IsActive = (bool)e.Value;
            await schedulerIndex.LoadCalendarViewData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                await LoadDataAsync();
                await LoadCategoris();
            }
        }

        async Task LoadCategoris()
        {
            flightCategories = await FlightCategoryService.ListAll(dependecyParams);
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
    }
}
