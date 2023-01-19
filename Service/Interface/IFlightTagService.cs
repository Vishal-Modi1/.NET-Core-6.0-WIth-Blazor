using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IFlightTagService
    {
        CurrentResponse List();

        CurrentResponse Create(FlightTagVM flightTagVM);

        CurrentResponse ListDropDownValues();
    }
}
