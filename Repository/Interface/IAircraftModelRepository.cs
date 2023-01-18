using DataModels.Entities;
using DataModels.VM.AircraftModel;
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

        List<DropDownValues> ListDropDownValues(int companyId);

        List<AircraftModelDataVM> List(DatatableParams datatableParams);

        AircraftModel FindByCondition(Expression<Func<AircraftModel, bool>> predicate);

        void Delete(int id);

        AircraftModel Edit(AircraftModel aircraftModel);

        bool IsAlreadyUsed(int id);

    }
}
