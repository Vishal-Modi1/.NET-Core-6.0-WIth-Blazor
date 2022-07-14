using DataModels.VM.Common;
using DataModels.VM.Location;

namespace Service.Interface
{
    public interface ILocationService
    {
        CurrentResponse Create(LocationVM locationVM);
        CurrentResponse ListDropDownValues();
        CurrentResponse Edit(LocationVM locationVM);
        CurrentResponse Delete(int id, long deletedBy);

        CurrentResponse List(DatatableParams datatableParams);

        CurrentResponse GetDetails(int id);
    }
}
