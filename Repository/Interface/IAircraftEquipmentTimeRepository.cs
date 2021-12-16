using DataModels.Entities;
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
        void DeleteEquipmentTimes(int airCraftId,int UpdatedBy);

        AircraftEquipmentTime FindByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);
        List<AircraftEquipmentTime> FindListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate);
    }
}
