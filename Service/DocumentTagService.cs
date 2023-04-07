using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (!documentTagVM.IsGlobalTag)
                {
                    bool isDocumentTagExist = IsTagExist(documentTagVM.Id, documentTagVM.TagName, documentTagVM.CompanyId.GetValueOrDefault());

                    if (isDocumentTagExist)
                    {
                        CreateResponse(null, HttpStatusCode.Ambiguous, "Document tag is already exist");

                        return _currentResponse;
                    }
                }
                else
                {
                    bool isDocumentTagExist = IsGlobalTagExist(documentTagVM.Id, documentTagVM.TagName);

                    if (isDocumentTagExist)
                    {
                        CreateResponse(null, HttpStatusCode.Ambiguous, "Document tag is already exist in some companies, Please try with different name");

                        return _currentResponse;
                    }

                    documentTagVM.CompanyId = null;
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

        public CurrentResponse Edit(DocumentTagVM documentTagVM)
        {
            try
            {
                if (!documentTagVM.IsGlobalTag)
                {
                    bool isDocumentTagExist = IsTagExist(documentTagVM.Id, documentTagVM.TagName, documentTagVM.CompanyId.GetValueOrDefault());

                    if (isDocumentTagExist)
                    {
                        CreateResponse(null, HttpStatusCode.Ambiguous, "Document tag is already exist");

                        return _currentResponse;
                    }
                }
                else
                {
                    bool isDocumentTagExist = IsGlobalTagExist(documentTagVM.Id, documentTagVM.TagName);

                    if (isDocumentTagExist)
                    {
                        CreateResponse(null, HttpStatusCode.Ambiguous, "Document tag is already exist in some companies, Please try with different name");

                        return _currentResponse;
                    }

                    documentTagVM.CompanyId = null;
                }


                DocumentTag documentTag = ToDataObject(documentTagVM);
                documentTag = _documentTagRepository.Edit(documentTag);
                CreateResponse(documentTag, HttpStatusCode.OK, "Document tag updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public bool IsGlobalTagExist(int id, string tag)
        {
            DocumentTagVM documentTag = _documentTagRepository.FindByCondition(p => p.TagName.ToLower() == tag.ToLower()
            && p.IsActive == true && p.IsDeleted == false && p.Id != id);

            if (documentTag == null)
            {
                return false;
            }

            return true;
        }

        public bool IsTagExist(int id, string tag, int companyId)
        {
            DocumentTagVM documentTag = _documentTagRepository.FindByCondition(p => p.TagName.ToLower() == tag.ToLower()
            && p.IsActive == true && p.IsDeleted == false && p.CompanyId == companyId && p.Id != id);

            if (documentTag == null)
            {
                return false;
            }

            return true;
        }

        public CurrentResponse ListByCompanyId(int companyId, long userId, string role)
        {
            try
            {
                List<DocumentTagDataVM> documentTagsList = _documentTagRepository.ListByCompanyId(companyId, userId);

                if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString())
                {
                    documentTagsList.ForEach(documentTag => { documentTag.IsAllowToEdit = true; });
                }
                else if (role.Replace(" ", "") == DataModels.Enums.UserRole.Admin.ToString())
                {
                    documentTagsList.Where(p=>p.CompanyId != null).ToList().ForEach(documentTag => { documentTag.IsAllowToEdit = true; });
                }
                else 
                {
                    documentTagsList.Where(p => p.CompanyId != null && p.CreatedBy == userId).ToList().ForEach(documentTag => { documentTag.IsAllowToEdit = true; });
                }

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

        public CurrentResponse FindById(int id)
        {
            try
            {
                DocumentTagVM documentTag = _documentTagRepository.FindByCondition(p => p.Id == id && p.IsActive && !p.IsDeleted);
                CreateResponse(documentTag, HttpStatusCode.OK, "");

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
            DocumentTag documentTag = _mapper.Map<DocumentTag>(documentTagVM);

            if (documentTagVM.Id > 0)
            {
                documentTag.UpdatedOn = DateTime.UtcNow;
            }

            return documentTag;
        }
    }
}
