using System.Collections.Generic;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.LogBook;

namespace Repository.Interface
{
    public interface ILogBookRepository : IBaseRepository<LogBook>
    {
        List<DropDownSmallValues> ListInstrumentApproachesDropdownValues();

        LogBookVM FindById(long id);

        LogBookVM Create(LogBookVM logBookVM);

        List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logbookId);

        void UpdateImagesName(long logbookId, List<LogBookFlightPhoto> logBookFlightPhotosList);
    }
}
