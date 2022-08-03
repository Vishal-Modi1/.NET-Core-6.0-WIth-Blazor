using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IAircraftStatusRepository : IBaseRepository<AircraftStatus>
    {
        List<DropDownValues> ListDropDownValues();

        List<AircraftStatus> ListAll();
    }
}
