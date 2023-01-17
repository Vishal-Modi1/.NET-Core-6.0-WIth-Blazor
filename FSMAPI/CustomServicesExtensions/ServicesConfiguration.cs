using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

namespace FSMAPI.CustomServicesExtensions
{
    public static class ServicesConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<IMyAccountService, MyAccountService>();
            services.AddScoped<IInstructorTypeService, InstructorTypeService>();
            services.AddScoped<IAircraftMakeService, AircraftMakeService>();
            services.AddScoped<IAircraftModelService, AircraftModelService>();
            services.AddScoped<IAircraftCategoryService, AircraftCategoryService>();
            services.AddScoped<IAircraftClassService, AircraftClassService>();
            services.AddScoped<IAircraftEquipementTimeService, AircraftEquipementTimeService>();
            services.AddScoped<IAircraftService, AircraftService>();
            services.AddScoped<IEquipmentStatusService, StatusService>();
            services.AddScoped<IEquipmentClassificationService, EquipmentClassificationService>();
            services.AddScoped<IAircraftEquipmentService, AircraftEquipmentService>();
            services.AddScoped<IUserRolePermissionService, UserRolePermissionService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAircraftScheduleService, AircraftScheduleService>();
            services.AddScoped<IAircraftScheduleDetailService, AircraftScheduleDetailService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IUserPreferenceService, UserPreferenceService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IDocumentTagService, DocumentTagService>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
            services.AddScoped<IModuleDetailsService, ModuleDetailsService>();
            services.AddScoped<IBillingHistoryService, BillingHistoryService>();
            services.AddScoped<IEmailTokenService, EmailTokenService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ITimezoneService, TimezoneService>();
            services.AddScoped<IUserVSCompanyService, UserVSCompanyService>();
            services.AddScoped<IAircraftStatusService, AircraftStatusService>();
            services.AddScoped<IInviteUserService, InviteUserService>();
            services.AddScoped<IDiscrepancyService, DiscrepancyService>();
            services.AddScoped<IDiscrepancyStatusService, DiscrepancyStatusService>();
            services.AddScoped<IDiscrepancyHistoryService, DiscrepancyHistoryService>();
            services.AddScoped<IDiscrepancyFileService, DiscrepancyFileService>();
            services.AddScoped<IEmailConfigurationService, EmailConfigurationService>();
            services.AddScoped<IAirTrafficControlCenterService, AirTrafficControlCenterService>();
            services.AddScoped<IUserAirTrafficControlCenterService, UserAirTrafficControlCenterService>();
            services.AddScoped<IWindyMapConfigurationService, WindyMapConfigurationService>();
            services.AddScoped<IBillingConfigurationService, BillingConfigurationService>();
            services.AddScoped<IFlightCategoryService, FlightCategoryService>();
            services.AddScoped<IAircraftLiveTrackerMapConfigurationService, AircraftLiveTrackerMapConfigurationService>();
            services.AddScoped<IRadarMapConfigurationService, RadarMapConfigurationService>();
            services.AddScoped<IVFRMapConfigurationService, VFRMapConfigurationService>();
        }

        public static void AddCustomRepositories(this IServiceCollection services)
        {
           //services.AddTransient<IRepository,BaseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IInstructorTypeRepository, InstructorTypeRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IEmailTokenRepository, EmailTokenRepository>();
            services.AddScoped<IMyAccountRepository, MyAccountRepository>();
            services.AddScoped<IAircraftMakeRepository, AircraftMakeRepository>();
            services.AddScoped<IAircraftModelRepository, AircraftModelRepository>();
            services.AddScoped<IAircraftCategoryRepository, AircraftCategoryRepository>();
            services.AddScoped<IAircraftClassRepository, AircraftClassRepository>();
            services.AddScoped<IAircraftEquipmentTimeRepository, AircraftEquipmentTimeRepository>();
            services.AddScoped<IAircraftRepository, AircraftRepository>();
            services.AddScoped<IEquipmentStatusRepository, StatusRepository>();
            services.AddScoped<IEquipmentClassificationRepository, EquipmentClassificationRepository>();
            services.AddScoped<IAircraftEquipmentRepository, AircraftEquipmentRepository>();
            services.AddScoped<IUserRolePermissionRepository, UserRolePermissionRepository>();
            services.AddScoped<IModuleDetailsRepository, ModuleDetailsRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAircraftScheduleRepository, AircraftScheduleRepository>();
            services.AddScoped<IAircraftScheduleDetailRepository, AircraftScheduleDetailRepository>();
            services.AddScoped<IAircraftScheduleHobbsTimeRepository, AircraftScheduleHobbsTimeRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentTagRepository, DocumentTagRepository>();
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<IBillingHistoryRepository, BillingHistoryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITimezoneRepository, TimezoneRepository>();
            services.AddScoped<IUserVSCompanyRepository, UserVSCompanyRepository>();
            services.AddScoped<IAircraftStatusRepository, AircraftStatusRepository>();
            services.AddScoped<IInviteUserRepository, InviteUserRepository>();
            services.AddScoped<IDiscrepancyRepository, DiscrepancyRepository>();
            services.AddScoped<IDiscrepancyStatusRepository, DiscrepancyStatusRepository>();
            services.AddScoped<IDiscrepancyHistoryRepository, DiscrepancyHistoryRepository>();
            services.AddScoped<IDiscrepancyFileRepository, DiscrepancyFileRepository>();
            services.AddScoped<IEmailConfigurationRepository, EmailConfigurationRepository>();
            services.AddScoped<IAirTrafficControlCenterRepository, AirTrafficControlCenterRepository>();
            services.AddScoped<IUserAirTrafficControlCenterRepository, UserAirTrafficControlCenterRepository>();
            services.AddScoped<IWindyMapConfigurationRepository, WindyMapConfigurationRepository>();
            services.AddScoped<IBillingConfigurationRepository, BillingConfigurationRepository>();
            services.AddScoped<IFlightCategoryRepository, FlightCategoryRepository>();
            services.AddScoped<IAircraftLiveTrackerMapConfigurationRepository, AircraftLiveTrackerMapConfigurationRepository>();
            services.AddScoped<IRadarMapConfigurationRepository, RadarMapConfigurationRepository>();
            services.AddScoped<IVFRMapConfigurationRepository, VFRMapConfigurationRepository>();
        }
    }
}
