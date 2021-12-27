using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface IAircraftRepository
    {

        AirCraft Create(AirCraft airCraft);

        List<AircraftDataVM> List(AircraftDatatableParams datatableParams);

        AirCraft Edit(AirCraft airCraft);

        AirCraft FindByCondition(Expression<Func<AirCraft, bool>> predicate);

        void Delete(int id);

        List<FlightSimulatorClass> FlightSimulatorClassList();

        bool UpdateImageName(int id, string imageName);

        List<DropDownValues> ListFlightSimulatorClassDropDownValues();
    }
}
