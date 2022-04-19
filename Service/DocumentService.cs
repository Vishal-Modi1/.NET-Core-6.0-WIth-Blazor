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

namespace Service
{
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IModuleDetailsRepository _moduleDetailsRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDocumentTagRepository _documentTagRepository;

        public DocumentService(IDocumentRepository documentRepository,
            IModuleDetailsRepository moduleDetailsRepository,
            ICompanyRepository companyRepository, IDocumentTagRepository documentTagRepository)
        {
            _documentRepository = documentRepository;
            _moduleDetailsRepository = moduleDetailsRepository;
            _companyRepository = companyRepository;
            _documentTagRepository = documentTagRepository;
        }

        public CurrentResponse Create(DocumentVM documentVM)
        {
            try
            {
                Document document = ToDataObject(documentVM);

                if (document.IsShareable)
                {
                    document.LastShareDate = null;
                }

                document.TagIds = GetDocumentTagIds(documentVM.Tags, documentVM.CreatedBy);

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

        private string GetDocumentTagIds(string documentTags, long createdBy)
        {
            string ids = "";

            string[] listTags = documentTags.Split(",",StringSplitOptions.RemoveEmptyEntries);

            List<DocumentTagVM> existingTagsList = _documentTagRepository.ListByCondition(p => p.IsActive == true && p.IsDeleted == false && listTags.Contains(p.TagName));

            List<DocumentTag> documentTagsList = new List<DocumentTag>();

            foreach (string tagName in listTags)
            {
                if(existingTagsList.Where(p=>p.TagName == tagName).Count() > 0)
                {
                    continue;
                }

                DocumentTag documentTag = new DocumentTag();

                documentTag.TagName = tagName;
                documentTag.CreatedOn = DateTime.UtcNow;
                documentTag.CreatedBy = createdBy;
                documentTag.IsActive = true;
                documentTagsList.Add(documentTag);
            }

             _documentTagRepository.Create(documentTagsList);

            existingTagsList = _documentTagRepository.ListByCondition(p => p.IsActive == true && p.IsDeleted == false  && listTags.Contains(p.TagName));

            ids = String.Join("," ,existingTagsList.Select(p => p.Id));

            return ids;
        }

        public CurrentResponse Edit(DocumentVM documentVM)
        {
            try
            {
                Document document = ToDataObject(documentVM);

                document.TagIds = GetDocumentTagIds(documentVM.Tags, documentVM.UpdatedBy.GetValueOrDefault());
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
                documentDataVM.DocumentPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectory.Document}/{documentDataVM.CompanyId}/{documentDataVM.UserId}/{documentDataVM.Name}";
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

        #region Object Mapping

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
            documentVM.TotalDownloads = document.TotalDownloads;
            documentVM.TotalShares = document.TotalShares;
            documentVM.LastShareDate = document.LastShareDate;
            documentVM.IsShareable = document.IsShareable;

            if (!string.IsNullOrWhiteSpace(document.TagIds))
            {
                List<int> listTagIds = document.TagIds.Split(",").Select(p => Convert.ToInt32(p)).ToList();
                List<DocumentTagVM> documentTags = _documentTagRepository.ListByCondition(p => p.IsActive == true && p.IsDeleted == false && listTagIds.Contains(p.Id));

               documentVM.Tags = String.Join(",",documentTags.Select(p => p.TagName).ToList());
            }

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
            document.LastShareDate = documentVM.LastShareDate;
            document.IsShareable = documentVM.IsShareable;

            document.CreatedBy = documentVM.CreatedBy;
           
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
