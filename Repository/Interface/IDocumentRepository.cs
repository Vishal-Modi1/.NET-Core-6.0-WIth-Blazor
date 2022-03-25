using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IDocumentRepository
    {
        Document Create(Document document);

        Document Edit(Document document);

        bool UpdateDocumentName(Guid id, string name);

        Document FindByCondition(Expression<Func<Document, bool>> predicate);

        List<DocumentDataVM> List(DocumentDatatableParams datatableParams);

        void Delete(Guid id);

        long UpdateTotalDownloads(Guid id);

        long UpdateTotalShares(Guid id);
    }
}
