using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IDocumentTagRepository
    {
        List<DocumentTagVM> List();

        DocumentTag Create(DocumentTag documentTag);

        List<DocumentTag> Create(List<DocumentTag> documentTagsList);

        List<DocumentTagVM> ListByCondition(Expression<Func<DocumentTag, bool>> predicate);

        DocumentTagVM FindByCondition(Expression<Func<DocumentTag, bool>> predicate);

        List<DropDownLargeValues> ListDropDownValues(int companyId);
    }
}
