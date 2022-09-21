using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IAircraftMakeService
    {
        CurrentResponse Create(AircraftMake aircraftMake);

        CurrentResponse List();

        CurrentResponse List(DatatableParams datatableParams);

        CurrentResponse Delete(int id);

        CurrentResponse Edit(AircraftMake aircraftMake);

        CurrentResponse ListDropDownValues();
    }
}
