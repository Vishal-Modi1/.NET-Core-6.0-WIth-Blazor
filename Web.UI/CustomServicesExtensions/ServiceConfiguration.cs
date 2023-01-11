using Web.UI.Data.Account;
using Web.UI.Data.Aircraft;
using Web.UI.Data.Aircraft.AircraftEquipment;
using Web.UI.Data.Common;
using Web.UI.Data.Company;
using Web.UI.Data.InstructorType;
using Web.UI.Data.User;
using Web.UI.Data.UserRolePermission;
using Web.UI.Data.AircraftMake;
using Web.UI.Data.MyAccount;
using Web.UI.Data.AircraftModel;
using Web.UI.Data.AircraftSchedule;
using Web.UI.Data.Reservation;
using Web.UI.Data.Document;
using Web.UI.Data.SubscriptionPlan;
using Web.UI.Data.ModuleDetail;
using Web.UI.Data.BillingHistory;
using Web.UI.Data.Location;
using Web.UI.Data.Timezone;
using Web.UI.Data.AircraftStatus;
using Web.UI.Data.InviteUser;
using Web.UI.Data.Airport;
using Web.UI.Data.Discrepancy;
using Web.UI.Data.Company.Settings;
using Web.UI.Data.Weather;

namespace Web.UI.CustomServicesExtensions
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<MenuService>();
            builder.Services.AddScoped<InstructorTypeService>();
            builder.Services.AddScoped<CompanyService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserRolePermissionService>();
            builder.Services.AddScoped<AircraftService>();
            builder.Services.AddScoped<AircraftEquipmentService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<MyAccountService>();
            builder.Services.AddScoped<AircraftMakeService>();
            builder.Services.AddScoped<AircraftModelService>();
            builder.Services.AddScoped<AircraftSchedulerService>();
            builder.Services.AddScoped<AircraftSchedulerDetailService>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<DocumentService>();
            builder.Services.AddScoped<SubscriptionPlanService>();
            builder.Services.AddScoped<ModuleDetailsService>();
            builder.Services.AddScoped<BillingHistoryService>();
            builder.Services.AddScoped<TokenValidatorService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<TimezoneService>();
            builder.Services.AddScoped<AircraftStatusService>();
            builder.Services.AddScoped<InviteUserService>();
            builder.Services.AddScoped<AirportService>();
            builder.Services.AddScoped<DiscrepancyService>();
            builder.Services.AddScoped<DiscrepancyFileService>();
            builder.Services.AddScoped<EmailConfigurationService>();
            builder.Services.AddScoped<AirTrafficControlCenterService>();
            builder.Services.AddScoped<WindyMapConfigurationService>();
            builder.Services.AddScoped<RadarMapConfigurationService>();
            builder.Services.AddScoped<AircraftLiveTrackerMapConfigurationService>();
            builder.Services.AddScoped<BillingConfigurationService>();
            builder.Services.AddScoped<FlightCategoryService>();
        }
    }
}
