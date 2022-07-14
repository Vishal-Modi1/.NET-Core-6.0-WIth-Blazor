using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftModelService
    {
        CurrentResponse Create(AircraftModel aircraftModel);

        CurrentResponse List();

        CurrentResponse List(DatatableParams datatableParams);

        CurrentResponse Delete(int id);

        CurrentResponse Edit(AircraftModel aircraftModel);
    }
}
