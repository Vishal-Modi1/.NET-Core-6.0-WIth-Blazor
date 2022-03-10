using FSM.Blazor.Data;
using FSM.Blazor.Data.Account;
using FSM.Blazor.Data.Aircraft;
using FSM.Blazor.Data.Aircraft.AircraftEquipment;
using FSM.Blazor.Data.Common;
using FSM.Blazor.Data.Company;
using FSM.Blazor.Data.InstructorType;
using FSM.Blazor.Data.User;
using FSM.Blazor.Data.UserRolePermission;
using FSM.Blazor.Data.AircraftMake;
using FSM.Blazor.Data.MyAccount;
using FSM.Blazor.Data.AircraftModel;
using FSM.Blazor.Data.AircraftSchedule;
using FSM.Blazor.Data.Reservation;
using FSM.Blazor.Data.Document;

namespace FSM.Blazor.CustomServicesExtensions
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
        }
    }
}
