using DataModels.VM.Common;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class TimezoneRepository : ITimezoneRepository
    {
        private MyContext _myContext;

        public List<DropDownValues> ListDropdownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> timezonesList = (from timezone in _myContext.Timezones
                                                   select new DropDownValues()
                                                   {
                                                       Id = timezone.Id,
                                                       Name = timezone.TimezoneName + " (" + timezone.Offset + ")" 
                                                   }).ToList();

                return timezonesList;
            }
        }
    }
}
