using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IDocumentTagRepository : IBaseRepository<DocumentTag>
    {
        List<DocumentTagDataVM> ListByCompanyId(int companyId, long userId);

        List<DocumentVsDocumentTag> Create(List<DocumentVsDocumentTag> documentTagsList, Guid documentId);

        List<DocumentTagVM> ListByCondition(Expression<Func<DocumentTag, bool>> predicate);

        DocumentTagVM FindByCondition(Expression<Func<DocumentTag, bool>> predicate);

        DocumentTag Edit(DocumentTag documentTag);

        List<DropDownLargeValues> ListDropDownValues(int companyId);

        List<DocumentVsDocumentTag> ListDocumentVsDocumentTagsByDocumentId(Guid documentId);
    }
}
