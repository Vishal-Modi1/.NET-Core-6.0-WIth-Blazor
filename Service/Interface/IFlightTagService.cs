using DataModels.VM.Common;
using DataModels.VM.Scheduler;

namespace Service.Interface
{
    public interface IFlightTagService
    {
        CurrentResponse List();

        CurrentResponse Create(FlightTagVM flightTagVM);

        CurrentResponse ListDropDownValues(int companyId);
    }
}
