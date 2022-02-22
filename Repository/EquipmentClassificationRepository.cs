using DataModels.Entities;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class EquipmentClassificationRepository : IEquipmentClassificationRepository
    {
        private MyContext _myContext;

        public List<EquipmentClassification> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.EquipmentClassifications.Where(c => c.IsActive == true).ToList();
            }
        }
    }
}
