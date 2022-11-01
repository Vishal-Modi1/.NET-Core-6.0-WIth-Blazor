using DataModels.VM.Common;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class DiscrepancyStatusRepository : IDiscrepancyStatusRepository
    {
        private MyContext _myContext;

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> discrepancyStatus = (from status in _myContext.DiscrepancyStatuses
                                                          select new DropDownValues()
                                                         {
                                                             Id = status.Id,
                                                             Name = status.Status
                                                         }).ToList();

                return discrepancyStatus;
            }
        }
    }
}
