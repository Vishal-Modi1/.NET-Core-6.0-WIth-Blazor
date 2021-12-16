using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftClassRepository
    {
        AircraftClass Create(AircraftClass aircraftClass);

        List<AircraftClass> List();

        List<DropDownValues> ListDropDownValues();

        AircraftClass FindByCondition(Expression<Func<AircraftClass, bool>> predicate);
    }
}
