using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using DataModels.VM.User;
using DataModels.VM.InstructorType;
using DataModels.VM.UserRolePermission;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Company;

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
                   .AddJsonFile("configuration.json")
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

        public DbSet<UserDataVM> UserSearchList { get; set; }

        public DbSet<UserRolePermissionDataVM> UserRolePermissionList { get; set; }

        public DbSet<EmailToken> EmailTokens { get; set; }

        public DbSet<InstructorTypeVM> InstructorType { get; set; }

        public DbSet<AirCraft> AirCrafts { get; set; }

        public DbSet<AircraftMake> AircraftMakes { get; set; }

        public DbSet<AircraftModel> AircraftModels { get; set; }
        
        public DbSet<AircraftEquipmentTime> AircraftEquipmentTimes { get; set; }

        public DbSet<AircraftCategory> AircraftCategories { get; set; }

        public DbSet<AircraftClass> AircraftClasses { get; set; }

        public DbSet<EquipmentStatus> EquipmentStatuses { get; set; }

        public DbSet<EquipmentClassification> EquipmentClassifications { get; set; }

        public DbSet<AirCraftEquipment> AircraftEquipments { get; set; }

        public DbSet<FlightSimulatorClass> FlightSimulatorClasses { get; set; }

        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<ModuleDetail> ModuleDetails { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyVM> CompanyData { get; set; }


        public DbSet<AircraftEquipmentDataVM> AircraftEquipmentData { get; set; }
    }

}
