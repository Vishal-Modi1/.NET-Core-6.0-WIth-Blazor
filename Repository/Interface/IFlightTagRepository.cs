using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document;
using DataModels.VM.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IFlightTagRepository
    {
        List<FlightTagVM> List();

        FlightTag Create(FlightTag flightTag);

        List<FlightTag> Create(List<FlightTag> flightTagsList);

        List<FlightTagVM> ListByCondition(Expression<Func<FlightTag, bool>> predicate);

        FlightTagVM FindByCondition(Expression<Func<FlightTag, bool>> predicate);

        List<DropDownLargeValues> ListDropDownValues();
    }
}
