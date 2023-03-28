using DataModels.VM.Common;
using DataModels.VM.Document.DocumentDirectory;

namespace Service.Interface
{
    public interface IDocumentDirectoryService
    {
        CurrentResponse Create(DocumentDirectoryVM documentDirectoryVM);
        CurrentResponse Edit(DocumentDirectoryVM documentDirectoryVM);
        CurrentResponse UpdateDocumentName(long id, string name);
        CurrentResponse GetDetails(long id, int companyId);
        CurrentResponse ListWithCountByComapnyId(int companyId);
        CurrentResponse Delete(long id, long deletedBy);
        CurrentResponse FindById(long id);
        CurrentResponse ListDropDownValuesByCompanyId(int companyId);
    }
}
