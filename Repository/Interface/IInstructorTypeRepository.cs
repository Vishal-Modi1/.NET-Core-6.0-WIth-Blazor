using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataModels.VM.InstructorType;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface IInstructorTypeRepository
    {
        List<InstructorType> List();

        InstructorType Create(InstructorType instructorType);

        List<InstructorTypeVM> List(DatatableParams datatableParams);

        InstructorType Edit(InstructorType instructorType);

        InstructorType FindByCondition(Expression<Func<InstructorType, bool>> predicate);

        void Delete(int id, long deletedBy);

        List<DropDownValues> ListDropDownValues();

    }
}
