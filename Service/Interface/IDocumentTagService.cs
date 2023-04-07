using DataModels.VM.Common;
using DataModels.VM.Document;

namespace Service.Interface
{
    public interface IDocumentTagService
    {
        CurrentResponse ListByCompanyId(int companyId, long userId, string role);

        CurrentResponse Create(DocumentTagVM documentTagVM);

        CurrentResponse Edit(DocumentTagVM documentTagVM);

        CurrentResponse ListDropDownValues(int companyId);

        CurrentResponse FindById(int id);
    }
}
