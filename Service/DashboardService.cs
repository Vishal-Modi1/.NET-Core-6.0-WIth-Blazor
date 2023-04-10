using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Dashboard;
using DataModels.VM.Document;
using DataModels.VM.LogBook;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogBookRepository _logBookRepository;
        private readonly IDocumentService _documentService;
        private readonly IModuleDetailsRepository _moduleDetailsRepository;

        public DashboardService(IReservationRepository reservationRepository, ILogBookRepository logBookRepository,
               IDocumentService documentService, IModuleDetailsRepository moduleDetailsRepository)
        {
            _reservationRepository = reservationRepository;
            _logBookRepository = logBookRepository;
            _moduleDetailsRepository = moduleDetailsRepository;
            _documentService = documentService;
        }

        public CurrentResponse PilotDashboardDetails(long userId, int companyId, DateTime userTime)
        {
            try
            {
                PilotDashboardVM pilotDashboardVM = new PilotDashboardVM();

                pilotDashboardVM.UpcomingFlights.UpcomingFlightsList = _reservationRepository.ListUpcomingFlightsByUserId(userId, userTime).Take(5).ToList();
                pilotDashboardVM.DocumentDetails.DocumentDataListVM = GetDocumentDetails(userId, companyId);
                pilotDashboardVM.LogBookDetails.LogBookDetailsListVM = GetLogBookDetails(userId, companyId);

                CreateResponse(pilotDashboardVM, System.Net.HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception ex)
            {
                CreateResponse(null, System.Net.HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }


        private List<DocumentDataVM> GetDocumentDetails(long userId, int companyId)
        {
            //ModuleDetail moduleDetail = _moduleDetailsRepository.FindByName("User");

            DocumentDatatableParams datatableParams = new DocumentDatatableParams();
            datatableParams.UserRole = DataModels.Enums.UserRole.PilotRenter;
            datatableParams.ModuleId = 0;
            datatableParams.UserId = userId;
            datatableParams.CompanyId = companyId;

            List<DocumentDataVM> documentsList = _documentService.ListDetails(datatableParams);

            return documentsList;
        }

        private List<LogBookDataVM> GetLogBookDetails(long userId, int companyId)
        {
            LogBookDatatableParams datatableParams = new LogBookDatatableParams();
            datatableParams.UserId = userId;
            datatableParams.CompanyId = companyId;

            List<LogBookDataVM> logBooksList = _logBookRepository.List(datatableParams);

            return logBooksList;
        }
    }
}
