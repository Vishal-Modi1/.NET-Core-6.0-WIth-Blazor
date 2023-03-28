using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document.DocumentDirectory;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IDocumentDirectoryRepository : IBaseRepository<DocumentDirectory>
    {
        DocumentDirectory Edit(DocumentDirectory documentDirectory);

        List<DocumentDirectorySummaryVM> ListWithCountByComapnyId(int companyId);

        void Delete(long id, long deletedBy);

        List<DropDownLargeValues> ListDropDownValuesByCompanyId(int companyId);
    }
}
