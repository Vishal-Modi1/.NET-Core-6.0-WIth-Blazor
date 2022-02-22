using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftCategoryRepository
    {
        AircraftCategory Create(AircraftCategory aircraftCategory);

        List<AircraftCategory> List();

        List<DropDownValues> ListDropDownValues();
        
        AircraftCategory FindByCondition(Expression<Func<AircraftCategory, bool>> predicate);
    }
}
