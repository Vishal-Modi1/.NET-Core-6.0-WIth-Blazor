using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Location;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface ILocationRepository
    {
        Location Create(Location location);

        List<LocationDataVM> List(DatatableParams datatableParams);

        Location Edit(Location location);

        void Delete(int id, long deletedBy);

        List<DropDownValues> ListDropDownValues();

        Location FindByCondition(Expression<Func<Location, bool>> predicate);
    }
}
