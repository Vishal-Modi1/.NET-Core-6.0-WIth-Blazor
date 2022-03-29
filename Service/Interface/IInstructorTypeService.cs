using DataModels.VM.InstructorType;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IInstructorTypeService
    {
        CurrentResponse Create(InstructorTypeVM instructorTypeVM);
        CurrentResponse List(DatatableParams datatableParams);
        CurrentResponse Edit(InstructorTypeVM instructorTypeVM);
        CurrentResponse GetDetails(int id);
        CurrentResponse Delete(int id, long deletedBy);
    }
}
