﻿using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface IAircraftRepository
    {

        Aircraft Create(Aircraft airCraft);

        List<AircraftDataVM> List(AircraftDatatableParams datatableParams);

        List<Aircraft> ListAllByCompanyId(int companyId);

        Aircraft Edit(Aircraft airCraft);

        Aircraft FindByCondition(Expression<Func<Aircraft, bool>> predicate);

        void Delete(long id, long deletedBy);

        List<FlightSimulatorClass> FlightSimulatorClassList();

        bool UpdateImageName(long id, string imageName);

        List<DropDownValues> ListFlightSimulatorClassDropDownValues();

        List<DropDownLargeValues> ListDropDownValues(int companyId);

        bool UpdateStatus(long id, byte statusId);

        bool UpdateIsLockedForUpdate(long id, bool isLock);
    }
}
