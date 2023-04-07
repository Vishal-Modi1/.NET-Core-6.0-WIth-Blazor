using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class DocumentTagRepository : BaseRepository<DocumentTag>, IDocumentTagRepository
    {
        private MyContext _myContext;
        private readonly IMapper _mapper;

        public DocumentTagRepository(MyContext dbContext, IMapper mappper)
            : base(dbContext)
        {
            this._myContext = dbContext;
            this._mapper = mappper;
        }

        public List<DocumentTagDataVM> ListByCompanyId(int companyId, long userId)
        {
            List<DocumentTagDataVM> list;

            string sql = $"EXEC dbo.GetDocumentTagsListForFilter {companyId}, {userId}";
            list = _myContext.DocumentTagDataList.FromSqlRaw(sql).ToList();

            return list;
        }

        public List<DocumentTagVM> ListByCondition(Expression<Func<DocumentTag, bool>> predicate)
        {
            List<DocumentTagVM> listTags = _myContext.DocumentTags.Where(predicate).ToList().Select(p =>
                                            new DocumentTagVM
                                            {
                                                Id = p.Id,
                                                TagName = p.TagName,

                                            }).ToList();

            return listTags;
        }

        public DocumentTagVM FindByCondition(Expression<Func<DocumentTag, bool>> predicate)
        {
            DocumentTagVM documentTag = _myContext.DocumentTags.Where(predicate).ToList().Select(p =>
                                            new DocumentTagVM
                                            {
                                                Id = p.Id,
                                                TagName = p.TagName,
                                                CompanyId = p.CompanyId,
                                                CreatedBy = p.CreatedBy
                                            }).FirstOrDefault();

            return documentTag;
        }

        public DocumentTag Edit(DocumentTag documentTag)
        {
            DocumentTag existingDocumentTag = _myContext.DocumentTags.Where(p => p.Id == documentTag.Id).FirstOrDefault();

            if (existingDocumentTag == null)
            {
                return documentTag;
            }

            _mapper.Map(documentTag, existingDocumentTag);
            _myContext.SaveChanges();

            return existingDocumentTag;
        }

        public List<DropDownLargeValues> ListDropDownValues(int companyId)
        {
            List<DropDownLargeValues> documentTagsList = (from documentTag in _myContext.DocumentTags
                                                          where documentTag.CompanyId == companyId
                                                          || documentTag.CompanyId == null
                                                          select new DropDownLargeValues()
                                                          {
                                                              Id = documentTag.Id,
                                                              Name = documentTag.TagName
                                                          }).ToList();

            return documentTagsList;
        }

        public List<DocumentVsDocumentTag> Create(List<DocumentVsDocumentTag> documentTagsList, Guid documentId)
        {
            var data = _myContext.DocumentVsDocumentTags.Where(p => p.DocumentId == documentId).ToList();
            _myContext.DocumentVsDocumentTags.RemoveRange(data);
            _myContext.SaveChanges();

            if (documentTagsList.Count > 0)
            {
                _myContext.DocumentVsDocumentTags.AddRange(documentTagsList);
                _myContext.SaveChanges();
            }

            return documentTagsList;
        }

        public List<DocumentVsDocumentTag> ListDocumentVsDocumentTagsByDocumentId(Guid documentId)
        {
         return _myContext.DocumentVsDocumentTags.Where(p => p.DocumentId == documentId).ToList();
        }
    }
}
