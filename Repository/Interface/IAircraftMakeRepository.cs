using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftMakeRepository
    {
        AircraftMake Create(AircraftMake aircraftMake);

        List<DropDownValues> ListDropDownValues();

        List<AircraftMake> List();

        AircraftMake FindByCondition(Expression<Func<AircraftMake, bool>> predicate);
    }
}
