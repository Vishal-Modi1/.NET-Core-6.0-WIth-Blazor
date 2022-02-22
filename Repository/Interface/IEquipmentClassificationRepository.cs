using DataModels.Entities;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IEquipmentClassificationRepository
    {
        List<EquipmentClassification> List();
    }
}
