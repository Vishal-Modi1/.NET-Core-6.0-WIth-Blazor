using DataModels.VM.Document;
using DataModels.VM.Common;
using System;

namespace Service.Interface
{
    public interface IDocumentService
    {
        CurrentResponse Create(DocumentVM documentVM);
        CurrentResponse Edit(DocumentVM documentVM);
        CurrentResponse UpdateDocumentName(Guid id, string name);
        CurrentResponse GetDetails(Guid id, int companyId);
        CurrentResponse List(DocumentDatatableParams datatableParams);
        CurrentResponse Delete(Guid id);

        DocumentVM FindById(Guid id);
        CurrentResponse GetFiltersValue();
    }
}
