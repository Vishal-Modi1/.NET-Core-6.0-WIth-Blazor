using DataModels.VM.Common;
using DataModels.VM.LogBook;
using DataModels.Entities;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface  ILogBookService
    {
        CurrentResponse ListInstrumentApproachesDropdownValues();

        CurrentResponse FindById(long id);

        CurrentResponse Create(LogBookVM logBook);

        List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logBookId);

        CurrentResponse UpdateImagesName(long logBookId, List<LogBookFlightPhoto> logBookFlightPhotosList);
    }
}
