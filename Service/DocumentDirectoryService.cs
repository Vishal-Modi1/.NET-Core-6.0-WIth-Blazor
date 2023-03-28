using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document.DocumentDirectory;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class DocumentDirectoryService : BaseService, IDocumentDirectoryService
    {
        private readonly IDocumentDirectoryRepository _documentDirectoryRepository;
        private readonly IMapper _mapper;

        public DocumentDirectoryService(IDocumentDirectoryRepository documentDirectoryRepository, IMapper mapper)
        {
            _documentDirectoryRepository = documentDirectoryRepository;
            _mapper = mapper;
        }

        public CurrentResponse Create(DocumentDirectoryVM documentDirectoryVM)
        {
            try
            {
                bool isDocumentDirectoryExistExist = IsDocumentDirectoryExist(documentDirectoryVM);

                if (isDocumentDirectoryExistExist)
                {
                    CreateResponse(documentDirectoryVM, HttpStatusCode.Ambiguous, "Directory is already exist");
                }
                else
                {
                    DocumentDirectory documentDirectory = _mapper.Map<DocumentDirectory>(documentDirectoryVM);
                    documentDirectory = _documentDirectoryRepository.Create(documentDirectory);
                    documentDirectoryVM = _mapper.Map<DocumentDirectoryVM>(documentDirectory);

                    CreateResponse(documentDirectoryVM, HttpStatusCode.OK, "Directory added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsDocumentDirectoryExist(DocumentDirectoryVM documentDirectoryVM)
        {
            if (_documentDirectoryRepository.FindByCondition(p => p.Name == documentDirectoryVM.Name && p.Id != documentDirectoryVM.Id) == null)
            {
                return false;
            }

            return true;
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _documentDirectoryRepository.Delete(id,deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Directory deleted successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(DocumentDirectoryVM documentDirectoryVM)
        {
            try
            {
                bool isDocumentDirectoryExistExist = IsDocumentDirectoryExist(documentDirectoryVM);

                if (isDocumentDirectoryExistExist)
                {
                    CreateResponse(documentDirectoryVM, HttpStatusCode.Ambiguous, "Directory is already exist");
                }
                else
                {
                    DocumentDirectory documentDirectory = _mapper.Map<DocumentDirectory>(documentDirectoryVM);

                    documentDirectory.UpdatedOn = DateTime.UtcNow;
                    documentDirectory = _documentDirectoryRepository.Edit(documentDirectory);
                    documentDirectoryVM = _mapper.Map<DocumentDirectoryVM>(documentDirectory);

                    CreateResponse(documentDirectoryVM, HttpStatusCode.OK, "Directory updated successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindById(long id)
        {
            try
            {
                DocumentDirectory documentDirectory = _documentDirectoryRepository.FindByCondition(p => p.Id == id);
                DocumentDirectoryVM documentDirectoryVM = _mapper.Map<DocumentDirectoryVM>(documentDirectory);

                CreateResponse(documentDirectoryVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(long id, int companyId)
        {
            throw new NotImplementedException();
        }

        public CurrentResponse ListWithCountByComapnyId(int companyId)
        {
            try
            {
                List<DocumentDirectorySummaryVM> documentDirectories = _documentDirectoryRepository.ListWithCountByComapnyId(companyId);

                CreateResponse(documentDirectories, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateDocumentName(long id, string name)
        {
            throw new NotImplementedException();
        }

        public CurrentResponse ListDropDownValuesByCompanyId(int companyId)
        {
            try
            {
                List<DropDownLargeValues> documentDirectories = _documentDirectoryRepository.ListDropDownValuesByCompanyId(companyId);

                CreateResponse(documentDirectories, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
