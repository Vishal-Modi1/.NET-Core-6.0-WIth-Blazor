using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataModels.VM.AircraftEquipment;

namespace Repository.Interface
{
    public interface IAircraftEquipmentRepository
    {
        AircraftEquipment Create(AircraftEquipment airCraft);

        AircraftEquipment Edit(AircraftEquipment airCraft);
        
        void Delete(int id, long deletedBy);

        List<AircraftEquipment> List();

        AircraftEquipment FindByCondition(Expression<Func<AircraftEquipment, bool>> predicate);

        List<AircraftEquipment> FindListByCondition(Expression<Func<AircraftEquipment, bool>> predicate);

        List<AircraftEquipmentDataVM> List(AircraftEquipmentDatatableParams datatableParams);
    }
}
