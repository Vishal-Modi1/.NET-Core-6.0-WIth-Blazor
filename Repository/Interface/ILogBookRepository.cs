using System.Collections.Generic;
using DataModels.Entities;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface ILogBookRepository : IBaseRepository<LogBook>
    {
        List<DropDownSmallValues> ListInstrumentApproachesDropdownValues();
    }
}
