using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FlightTagRepository : IFlightTagRepository
    {
        private MyContext _myContext;

        public FlightTag Create(FlightTag flightTag)
        {
            using (_myContext = new MyContext())
            {
                _myContext.FlightTags.Add(flightTag);
                _myContext.SaveChanges();

                return flightTag;
            }
        }

        public List<FlightTag> Create(List<FlightTag> flightTagsList)
        {
            using (_myContext = new MyContext())
            {
                _myContext.FlightTags.AddRange(flightTagsList);
                _myContext.SaveChanges();

                return flightTagsList;
            }
        }

        public FlightTagVM FindByCondition(Expression<Func<FlightTag, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                FlightTagVM flightTag = _myContext.FlightTags.Where(predicate).ToList().Select(p =>
                                                new FlightTagVM
                                                {
                                                    Id = p.Id,
                                                    TagName = p.TagName,

                                                }).FirstOrDefault();

                return flightTag;
            }
        }

        public List<FlightTagVM> List()
        {
            using (_myContext = new MyContext())
            {
                List<FlightTagVM> listTags = _myContext.FlightTags.Where(f => f.IsActive == true && f.IsDeleted == false
                                                ).ToList().Select(f =>
                                                new FlightTagVM
                                                {
                                                    Id = f.Id,
                                                    TagName = f.TagName,

                                                }).ToList();

                return listTags;
            }
        }

        public List<FlightTagVM> ListByCondition(Expression<Func<FlightTag, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                List<FlightTagVM> listTags = _myContext.FlightTags.Where(predicate).ToList().Select(f =>
                                                new FlightTagVM
                                                {
                                                    Id =f.Id,
                                                    TagName = f.TagName,

                                                }).ToList();

                return listTags;
            }
        }

        public List<DropDownLargeValues> ListDropDownValues(int companyId)
        {
            using (_myContext = new MyContext())
            {
                List<DropDownLargeValues> flightTagsList = (from flightTag in _myContext.FlightTags
                                                            where flightTag.CompanyId == companyId
                                                              select new DropDownLargeValues()
                                                              {
                                                                  Id = flightTag.Id,
                                                                  Name = flightTag.TagName
                                                              }).ToList();

                return flightTagsList;
            }
        }
    }
}
