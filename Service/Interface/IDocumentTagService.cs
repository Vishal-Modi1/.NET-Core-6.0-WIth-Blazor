using DataModels.VM.Common;
using DataModels.VM.Document;

namespace Service.Interface
{
    public interface IDocumentTagService
    {
        CurrentResponse List();

        CurrentResponse Create(DocumentTagVM documentTagVM);

        CurrentResponse ListDropDownValues(int companyId);
    }
}
