using DataModels.Entities;
using DataModels.VM.Aircraft;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.BillingHistory;
using DataModels.VM.Company;
using DataModels.VM.Document;
using DataModels.VM.InstructorType;
using DataModels.VM.Location;
using DataModels.VM.Reservation;
using DataModels.VM.SubscriptionPlan;
using DataModels.VM.User;
using DataModels.VM.UserRolePermission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Repository
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public MyContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<InstructorType> InstructorTypes { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<BillingHistory> BillingHistories { get; set; }

        public DbSet<UserDataVM> UserSearchList { get; set; }

        public DbSet<BillingHistoryDataVM> BillingHistoryList { get; set; }

        public DbSet<SubscriptionPlanDataVM> SubscriptionPlanData { get; set; }

        public DbSet<UserRolePermissionDataVM> UserRolePermissionList { get; set; }

        public DbSet<EmailToken> EmailTokens { get; set; }

        public DbSet<InstructorTypeVM> InstructorType { get; set; }

        public DbSet<Aircraft> AirCrafts { get; set; }

        public DbSet<AircraftMake> AircraftMakes { get; set; }

        public DbSet<AircraftModel> AircraftModels { get; set; }
        
        public DbSet<AircraftEquipmentTime> AircraftEquipmentTimes { get; set; }

        public DbSet<AircraftCategory> AircraftCategories { get; set; }

        public DbSet<AircraftClass> AircraftClasses { get; set; }

        public DbSet<EquipmentStatus> EquipmentStatuses { get; set; }

        public DbSet<EquipmentClassification> EquipmentClassifications { get; set; }

        public DbSet<AircraftEquipment> AircraftEquipments { get; set; }

        public DbSet<FlightSimulatorClass> FlightSimulatorClasses { get; set; }

        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<ModuleDetail> ModuleDetails { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        public DbSet<CompanyVM> CompanyData { get; set; }
        public DbSet<ReservationDataVM> ReservationDataVM { get; set; }
        public DbSet<DocumentDataVM> DocumentDataVM { get; set; }

        public DbSet<AircraftDataVM> AircraftDataVMs { get; set; }

        public DbSet<AircraftEquipmentDataVM> AircraftEquipmentData { get; set; }
        public DbSet<ScheduleActivityType> ScheduleActivityTypes { get; set; }

        public DbSet<UserRoleVsScheduleActivityType> UserRoleVsScheduleActivityType { get; set; }

        public DbSet<AircraftSchedule> AircraftSchedules { get; set; }

        public DbSet<AircraftScheduleDetail> AircraftScheduleDetails { get; set; }

        public DbSet<AircraftScheduleHobbsTime> AircraftScheduleHobbsTimes { get; set; }
    
        public DbSet<Document> Documents { get; set; }
        
        public DbSet<DocumentTag> DocumentTags { get; set; }
        
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        
        public DbSet<CompanyService> CompanyServices { get; set; }
        
        public DbSet<Location> Locations { get; set; }

        public DbSet<Timezone> Timezones { get; set; }
    
        public DbSet<LocationDataVM> LocationsList { get; set; }
    }

}
