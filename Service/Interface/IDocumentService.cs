using DataModels.VM.Document;
using DataModels.VM.Common;
using System;
using System.Linq.Expressions;
using DataModels.Entities;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IDocumentService
    {
        CurrentResponse Create(DocumentVM documentVM);
        CurrentResponse Edit(DocumentVM documentVM);
        CurrentResponse UpdateDocumentName(Guid id, string name);
        CurrentResponse GetDetails(Guid id, int companyId);
        CurrentResponse List(DocumentDatatableParams datatableParams);
        CurrentResponse Delete(Guid id, long deletedBy);
        DocumentVM FindById(Guid id);
        CurrentResponse GetFiltersValue(int companyId);
        CurrentResponse FindByCondition(Expression<Func<Document, bool>> predicate);
        CurrentResponse UpdateTotalDownloads(Guid id);
        CurrentResponse UpdateTotalShares(Guid id);
        List<DocumentDataVM> ListDetails(DocumentDatatableParams datatableParams);
    }
}
