using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface ITimezoneRepository
    {
        List<DropDownValues> ListDropdownValues();
    }
}
