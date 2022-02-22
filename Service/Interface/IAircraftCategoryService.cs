using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftCategoryService
    {
        CurrentResponse Create(AircraftCategory  aircraftCategory);

        CurrentResponse List();
    }
}
