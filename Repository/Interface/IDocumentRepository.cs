using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Document Edit(Document document);

        bool UpdateDocumentName(Guid id, string name);

        List<DocumentDataVM> List(DocumentDatatableParams datatableParams);

        void Delete(Guid id, long deletedBy);

        long UpdateTotalDownloads(Guid id);

        long UpdateTotalShares(Guid id);
    }
}
