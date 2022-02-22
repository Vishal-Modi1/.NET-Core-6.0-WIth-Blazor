using DataModels.Entities;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IEquipmentStatusRepository
    {
        List<EquipmentStatus> List();
    }
}
