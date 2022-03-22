using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class DocumentTagService : BaseService, IDocumentTagService
    {
        private readonly IDocumentTagRepository _documentTagRepository;

        public DocumentTagService(IDocumentTagRepository documentTagRepository)
        {
            _documentTagRepository = documentTagRepository;
        }

        public CurrentResponse Create(DocumentTagVM documentTagVM)
        {
            try
            {
                bool isDocumentTagExist = IsTagExist(documentTagVM.TagName);

                if(isDocumentTagExist)
                {
                    CreateResponse(null, HttpStatusCode.Ambiguous, "Document tag is already exist");

                    return _currentResponse;
                }

                DocumentTag documentTag = ToDataObject(documentTagVM);
                documentTag = _documentTagRepository.Create(documentTag);
                CreateResponse(documentTag, HttpStatusCode.OK, "Document tag added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public bool IsTagExist(string tag)
        {
            DocumentTagVM documentTag = _documentTagRepository.FindByCondition(p => p.TagName.ToLower() == tag.ToLower() && p.IsActive == true && p.IsDeleted == false);

            if (documentTag == null)
            {
                return false;
            }

            return true;
        }

        public CurrentResponse List()
        {
            try
            {
                List<DocumentTagVM> documentTagsList = _documentTagRepository.List();
                CreateResponse(documentTagsList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private DocumentTag ToDataObject(DocumentTagVM documentTagVM)
        {
            DocumentTag documentTag = new DocumentTag();

            documentTag.TagName = documentTagVM.TagName;
            documentTag.IsActive = true;

            documentTag.CreatedBy = documentTagVM.CreatedBy;

            if (documentTagVM.Id == 0)
            {
                documentTag.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                documentTag.UpdatedBy = documentTagVM.UpdatedBy;

                documentTag.UpdatedOn = DateTime.UtcNow;
            }

            return documentTag;
        }
    }
}
