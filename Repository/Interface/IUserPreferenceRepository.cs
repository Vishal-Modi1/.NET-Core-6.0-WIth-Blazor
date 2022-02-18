using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IUserPreferenceRepository
    {
        UserPreference Create(UserPreference userPreference);

        //List<AircraftModel> List();

        //AircraftModel FindByCondition(Expression<Func<AircraftModel, bool>> predicate);

    }
}
