using DataModels.Entities;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class StatusRepository : IEquipmentStatusRepository
    {
        private MyContext _myContext;

        public List<EquipmentStatus> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.EquipmentStatuses.Where(c => c.IsActive == true).ToList();
            }
        }
    }
}
