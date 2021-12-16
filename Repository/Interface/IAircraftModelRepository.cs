using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftModelRepository
    {
        AircraftModel Create(AircraftModel aircraftModel);

        List<AircraftModel> List();

        List<DropDownValues> ListDropDownValues();


        AircraftModel FindByCondition(Expression<Func<AircraftModel, bool>> predicate);

    }
}
