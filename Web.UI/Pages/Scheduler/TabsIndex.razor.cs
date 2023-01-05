﻿using DataModels.Entities;
using DataModels.VM.Scheduler;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Utilities;
using Web.UI.Utilities;
using DataModels.VM.Reservation;

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
        FlightCategory _flightCategory;
        public List<UpcomingFlight> upcomingFlights = new();
        protected override Task OnInitializedAsync()
        {
            isLeftBarVisible = true;
            isFilterBarVisible = true;
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

                ChangeLoaderVisibilityAction(true);
                        
                await LoadDataAsync();
                await LoadCategoris();
                await LoadUpcomingFlights();

                ChangeLoaderVisibilityAction(false);
            }
        }

        private async Task LoadUpcomingFlights()
        {
            upcomingFlights = await ReservationService.ListUpcomingFlightsByCompanyId(dependecyParams, globalMembers.CompanyId);
            upcomingFlights.ForEach(p =>
            {
                p.StartDate = DateConverter.ToLocal(p.StartDate, globalMembers.Timezone);

            });
        }

        async Task LoadCategoris()
        {
            flightCategories = await FlightCategoryService.ListAll(dependecyParams);
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

        void OpenCreateCategoryDialogAsync(FlightCategory flightCategory)
        {
            isDisplayPopup = true;
            _flightCategory = flightCategory;

            if (flightCategory.Id == 0)
            {
                popupTitle = "Edit Category";
            }
            else
            {
                popupTitle = "Create Category";
            }
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
