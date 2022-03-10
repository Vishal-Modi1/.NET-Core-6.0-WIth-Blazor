using DataModels.Entities;
using DataModels.VM.Document;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using System.Collections.Generic;
using DataModels.Constants;

namespace Service
{
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IModuleDetailsRepository _moduleDetailsRepository;
        private readonly ICompanyRepository _companyRepository;

        public DocumentService(IDocumentRepository documentRepository,
            IModuleDetailsRepository moduleDetailsRepository,
            ICompanyRepository companyRepository)
        {
            _documentRepository = documentRepository;
            _moduleDetailsRepository = moduleDetailsRepository;
            _companyRepository = companyRepository;
        }

        public CurrentResponse Create(DocumentVM documentVM)
        {
            try
            {
                Document document = ToDataObject(documentVM);

                document = _documentRepository.Create(document);
                CreateResponse(document, HttpStatusCode.OK, "Document details added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(DocumentVM documentVM)
        {
            try
            {
                Document document = ToDataObject(documentVM);

                document = _documentRepository.Edit(document);
                CreateResponse(document, HttpStatusCode.OK, "Document details updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(Guid id, int companyId)
        {
            try
            {
                Document document = _documentRepository.FindByCondition(p => p.Id == id && p.IsActive == true && p.IsDeleted == false);

                DocumentVM documentVM = new DocumentVM();

                if (document != null)
                {
                    documentVM = ToBusinessObject(document);
                }

                documentVM.ModulesList = _moduleDetailsRepository.ListDropDownValues();

                CreateResponse(documentVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public DocumentVM FindById(Guid id)
        {
            DocumentVM documentVM = new DocumentVM();

            try
            {
                Document document = _documentRepository.FindByCondition(p => p.Id == id && p.IsActive == true && p.IsDeleted == false);

                if (document != null)
                {
                    documentVM = ToBusinessObject(document);
                }

                return documentVM;
            }
            catch (Exception exc)
            {
                return documentVM;
            }
        }

        public CurrentResponse GetFiltersValue()
        {
            try
            {
                DocumentFilterVM documentFilterVM = new DocumentFilterVM();

                documentFilterVM.Companies = _companyRepository.ListDropDownValues();
                documentFilterVM.ModulesList = _moduleDetailsRepository.ListDropDownValues();

                CreateResponse(documentFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new DocumentFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateDocumentName(Guid id, string name)
        {
            try
            {
                bool isImageNameUpdated = _documentRepository.UpdateDocumentName(id, name);

                CreateResponse(isImageNameUpdated, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(DocumentDatatableParams datatableParams)
        {
            try
            {
                List<DocumentDataVM> documentsList = _documentRepository.List(datatableParams);

                foreach (DocumentDataVM documentDataVM in documentsList)
                {
                    documentDataVM.DocumentPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectory.Document}/{documentDataVM.CompanyId}/{documentDataVM.Name}";
                }

                CreateResponse(documentsList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(Guid id)
        {
            try
            {
                _documentRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Document deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private DocumentVM ToBusinessObject(Document document)
        {
            DocumentVM documentVM = new DocumentVM();

            documentVM.Id = document.Id;
            documentVM.Name = document.Name;
            documentVM.DisplayName = document.DisplayName;
            documentVM.ExpirationDate = document.ExpirationDate;
            documentVM.CompanyId = document.CompanyId;
            documentVM.UserId = document.UserId;
            documentVM.ModuleId = document.ModuleId;
            documentVM.Type = document.Type;

            return documentVM;
        }

        private Document ToDataObject(DocumentVM documentVM)
        {
            Document document = new Document();

            document.Id = documentVM.Id;
            document.Name = documentVM.Name == null ? "" : documentVM.Name;
            document.DisplayName = documentVM.DisplayName == null ? "" : documentVM.DisplayName;
            document.ExpirationDate = documentVM.ExpirationDate;
            document.CompanyId = documentVM.CompanyId;
            document.UserId = documentVM.UserId;
            document.ModuleId = documentVM.ModuleId;
            document.Type = documentVM.Type == null ? "" : documentVM.Type;
            document.Size = documentVM.Size;
            document.IsActive = true;

            document.CreatedBy = documentVM.CreatedBy;
            document.UpdatedBy = documentVM.UpdatedBy;

            if (documentVM.Id == Guid.Empty)
            {
                document.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                document.UpdatedOn = DateTime.UtcNow;
            }

            return document;
        }
    }
}
