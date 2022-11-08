using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using System;
using System.Linq.Expressions;

namespace Service.Interface
{
    public interface IDiscrepancyFileService
    {
        CurrentResponse Create(DiscrepancyFileVM discrepancyFileVM);

        CurrentResponse Edit(DiscrepancyFileVM discrepancyFileVM);

        CurrentResponse Delete(long id);

        CurrentResponse List(long discrepancyId);

        CurrentResponse GetDetails(long id);

        CurrentResponse FindByCondition(Expression<Func<DiscrepancyFile, bool>> predicate);

        CurrentResponse UpdateDocumentName(long id, string name);
    }
}
