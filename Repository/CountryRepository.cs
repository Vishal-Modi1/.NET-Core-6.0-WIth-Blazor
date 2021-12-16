using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class CountryRepository : ICountryRepository
    {
        private MyContext _myContext;

        public List<Country> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Countries.ToList();
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> countriesList = (from country in _myContext.Countries
                                                     select new DropDownValues()
                                                     {
                                                         Id = country.Id,
                                                         Name = country.Name
                                                     }).ToList();

                return countriesList;
            }
        }
    }
}
