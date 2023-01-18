using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Company.Settings;
using DataModels.VM.Scheduler;
using DataModels.VM.Weather;

namespace FSMAPI.Utilities
{
    public class AutoMappingHelper : Profile
    {
        public AutoMappingHelper()
        {
            CreateMap<FlightCategoryVM, FlightCategory>().ReverseMap();
            CreateMap<WindyMapConfigurationVM, WindyMapConfiguration>().ReverseMap();
            CreateMap<AircraftLiveTrackerMapConfigurationVM, AircraftLiveTrackerMapConfiguration>().ReverseMap();
            CreateMap<RadarMapConfigurationVM, RadarMapConfiguration>().ReverseMap();
            CreateMap<CompanyDateFormatVM, CompanyDateFormat>().ReverseMap();
            CreateMap<VFRMapConfigurationVM, VFRMapConfiguration>().ReverseMap();
        }
    }
}
