using DataModels.Entities;
using DataModels.VM.Document;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using System.Collections.Generic;
using DataModels.Constants;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace Service
{
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IModuleDetailsRepository _moduleDetailsRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDocumentTagRepository _documentTagRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DocumentService(IDocumentRepository documentRepository, IMapper mapper,
            IModuleDetailsRepository moduleDetailsRepository, IUserRepository userRepository,
            ICompanyRepository companyRepository, IDocumentTagRepository documentTagRepository)
        {
            _documentRepository = documentRepository;
            _moduleDetailsRepository = moduleDetailsRepository;
            _companyRepository = companyRepository;
            _documentTagRepository = documentTagRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public CurrentResponse Create(DocumentVM documentVM)
        {
            try
            {
                Document document = ToDataObject(documentVM);

                if (!document.IsShareable)
                {
                    document.LastShareDate = null;
                }

                document = _documentRepository.Create(document);
                List<DocumentVsDocumentTag> tags = GetDocumentTags(documentVM.Tags, document.Id);
                _documentTagRepository.Create(tags, document.Id);

                CreateResponse(document, HttpStatusCode.OK, "Document details added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private List<DocumentVsDocumentTag> GetDocumentTags(string documentTags, Guid documentId)
        {
            string ids = "";

            List<DocumentVsDocumentTag> tags = new List<DocumentVsDocumentTag>();
            string[] listTags = documentTags.Split(",",StringSplitOptions.RemoveEmptyEntries);

            foreach (string tag in listTags)
            {
                tags.Add(new DocumentVsDocumentTag() { DocumentId = documentId,DocumentTagId = Convert.ToInt32(tag)});
            }

            return tags;
        }

        public CurrentResponse Edit(DocumentVM documentVM)
        {
            try
            {
                Document document = ToDataObject(documentVM);

                document = _documentRepository.Edit(document);
                List<DocumentVsDocumentTag> tags = GetDocumentTags(documentVM.Tags, document.Id);
                _documentTagRepository.Create(tags, document.Id);
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

                if (documentVM.CompanyId == 0)
                {
                    documentVM.CompanyId = companyId;
                }

                documentVM.ModulesList = _moduleDetailsRepository.ListDropDownValues();

                if(documentVM.Id == Guid.Empty)
                {
                    documentVM.IsShareable = true;
                }

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

        public CurrentResponse GetFiltersValue(int companyId)
        {
            try
            {
                DocumentFilterVM documentFilterVM = new DocumentFilterVM();

                if (companyId == 0)
                {
                    documentFilterVM.Companies = _companyRepository.ListDropDownValues();
                }
                else
                {
                    documentFilterVM.UsersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);
                }

                documentFilterVM.ModulesList = _moduleDetailsRepository.ListDropDownValues();

                SetDocumentTypesList(documentFilterVM);

                CreateResponse(documentFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new DocumentFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindByCondition(Expression<Func<Document, bool>> predicate)
        {
            try
            {
                Document document = _documentRepository.FindByCondition(predicate);

                CreateResponse(document, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateDocumentName(Guid id, string name)
        {
            try
            {
                bool isImageNameUpdated = _documentRepository.UpdateDocumentName(id, name);
                Document document = _documentRepository.FindByCondition(p => p.Id == id);

                CreateResponse(document, HttpStatusCode.OK, "Document updated successfully.");

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
                List<DocumentDataVM> documentsList = ListDetails(datatableParams);
               
                CreateResponse(documentsList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public List<DocumentDataVM> ListDetails(DocumentDatatableParams datatableParams)
        {
            List<DocumentDataVM> documentsList = _documentRepository.List(datatableParams);

            foreach (DocumentDataVM documentDataVM in documentsList)
            {
                documentDataVM.DocumentPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.Document}/{documentDataVM.CompanyId}/{documentDataVM.UserId}/{documentDataVM.Name}";
            }

            return documentsList;
        }

        public CurrentResponse Delete(Guid id, long deletedBy)
        {
            try
            {
                _documentRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Document deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateTotalDownloads(Guid id)
        {
            try
            {
                long totalDownloads = _documentRepository.UpdateTotalDownloads(id);
                CreateResponse(totalDownloads, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(0, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateTotalShares(Guid id)
        {
            try
            {
                long totalShares = _documentRepository.UpdateTotalShares(id);
                CreateResponse(totalShares, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(0, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private void SetDocumentTypesList(DocumentFilterVM documentFilterVM)
        {
            documentFilterVM.TypesList.Add(new DropDownValues() { Id = 1, Name = "Excel" });
            documentFilterVM.TypesList.Add(new DropDownValues() { Id = 2, Name = "Word" });
            documentFilterVM.TypesList.Add(new DropDownValues() { Id = 3, Name = "Text" });
            documentFilterVM.TypesList.Add(new DropDownValues() { Id = 4, Name = "Image" });
            documentFilterVM.TypesList.Add(new DropDownValues() { Id = 5, Name = "PDF" });
        }

        #region Object Mapping

        private DocumentVM ToBusinessObject(Document document)
        {

            DocumentVM documentVM = _mapper.Map<DocumentVM>(document);
            documentVM.DocumentVsDocumentTags = _documentTagRepository.ListDocumentVsDocumentTagsByDocumentId(documentVM.Id);

            if(documentVM.DocumentVsDocumentTags != null)
            {
                documentVM.Tags = String.Join(",", documentVM.DocumentVsDocumentTags.Select(p => p.DocumentTagId).ToList());
            }
            

            return documentVM;
        }

        private Document ToDataObject(DocumentVM documentVM)
        {
            Document document = _mapper.Map<Document>(documentVM);
            if (documentVM.Id == Guid.Empty)
            {
                document.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                document.UpdatedOn = DateTime.UtcNow;
                document.UpdatedBy = documentVM.UpdatedBy;
            }

            return document;
        }

        #endregion
    }
}
