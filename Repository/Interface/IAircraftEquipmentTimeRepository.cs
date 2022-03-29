using DataModels.Entities;
using DataModels.VM.AircraftEquipment;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IAircraftEquipmentTimeRepository
    {
        AircraftEquipmentTime Create(AircraftEquipmentTime airCraft);

        AircraftEquipmentTime Edit(AircraftEquipmentTime airCraft);
        
        void Delete(int id, long deletedBy);
        void DeleteEquipmentTimes(long airCraftId,long UpdatedBy);

        AircraftEquipmentTime FindByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);

        List<AircraftEquipmentTime> FindListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);

        List<AircraftEquipmentTimeVM> ListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);
    }
}
