﻿using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using System;
using System.Linq.Expressions;

namespace Service.Interface
{
    public interface IDiscrepancyService
    {
        CurrentResponse Create(DiscrepancyVM discrepancyVM, string timezone);
        CurrentResponse Edit(DiscrepancyVM discrepancyVM, string timezone);
        CurrentResponse List(DiscrepancyDatatableParams datatableParams);
        CurrentResponse GetDetails(long id);
        Discrepancy FindByCondition(Expression<Func<Discrepancy, bool>> predicate);
    }
}
