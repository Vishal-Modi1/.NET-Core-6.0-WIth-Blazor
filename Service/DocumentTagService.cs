using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
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
        private readonly IMapper _mapper;

        public DocumentTagService(IDocumentTagRepository documentTagRepository, IMapper mapper)
        {
            _documentTagRepository = documentTagRepository;
            _mapper = mapper;
        }

        public CurrentResponse Create(DocumentTagVM documentTagVM)
        {
            try
            {
                bool isDocumentTagExist = IsTagExist(documentTagVM.TagName, documentTagVM.CompanyId);

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

        public bool IsTagExist(string tag, int companyId)
        {
            DocumentTagVM documentTag = _documentTagRepository.FindByCondition(p => p.TagName.ToLower() == tag.ToLower() 
            && p.IsActive == true && p.IsDeleted == false && p.CompanyId == companyId);

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

        public CurrentResponse ListDropDownValues(int companyId)
        {
            try
            {
                List<DropDownLargeValues> documentTagsList = _documentTagRepository.ListDropDownValues(companyId);
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
            documentTag = _mapper.Map<DocumentTag>(documentTagVM);
            documentTag.IsActive = true;

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
