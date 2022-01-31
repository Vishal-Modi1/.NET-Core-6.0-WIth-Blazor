using DataModels.Entities;
using DataModels.VM.AircraftEquipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IAircraftEquipmentTimeRepository
    {
        AircraftEquipmentTime Create(AircraftEquipmentTime airCraft);

        AircraftEquipmentTime Edit(AircraftEquipmentTime airCraft);
        
        void Delete(int id);
        void DeleteEquipmentTimes(long airCraftId,long UpdatedBy);

        AircraftEquipmentTime FindByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);

        List<AircraftEquipmentTime> FindListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);

        List<AircraftEquipmentTimeVM> ListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);
    }
}
