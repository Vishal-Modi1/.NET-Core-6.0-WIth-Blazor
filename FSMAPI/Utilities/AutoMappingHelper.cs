using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Company.Settings;
using DataModels.VM.Document;
using DataModels.VM.Document.DocumentDirectory;
using DataModels.VM.LogBook;
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
            CreateMap<NOAARadarMapConfigurationVM, NOAARadarMapConfiguration>().ReverseMap();
            CreateMap<CompanyDateFormatVM, CompanyDateFormat>().ReverseMap();
            CreateMap<VFRMapConfigurationVM, VFRMapConfiguration>().ReverseMap();
            CreateMap<FlightTagVM, FlightTag>().ReverseMap();
            CreateMap<DocumentTagVM, DocumentTag>().ReverseMap();
            CreateMap<LogBookVM, LogBook>().ReverseMap();
            CreateMap<LogBookTrainingDetailVM, LogBookTrainingDetail>().ReverseMap();
            CreateMap<LogBookInstrumentVM, LogBookInstrument>().ReverseMap();
            CreateMap<LogBookInstrumentApproachVM, LogBookInstrumentApproach>().ReverseMap();
            CreateMap<LogBookFlightTimeDetailVM, LogBookFlightTimeDetail>().ReverseMap();
            CreateMap<LogBookFlightPhotoVM, LogBookFlightPhoto>().ReverseMap();
            CreateMap<LogBookCrewPassengerVM, LogBookCrewPassenger>().ReverseMap();
            CreateMap<CrewPassengerVM, CrewPassenger>().ReverseMap();
            CreateMap<DocumentDirectoryVM, DocumentDirectory>().ReverseMap();
            CreateMap<DocumentVM, Document>().ReverseMap();
            CreateMap<Document, Document>().ReverseMap();
            CreateMap<DocumentTag, DocumentTag>().ReverseMap();
        }
    }
}
