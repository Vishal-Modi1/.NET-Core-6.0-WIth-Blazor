using DataModels.VM.Common;
using DataModels.VM.Company;
using System.Collections.Generic;

namespace DataModels.VM.Reservation
{
    public class ReservationFilterVM : CompanyFilterVM
    {
        public List<DropDownLargeValues> Users { get; set; }

        public long UserId { get; set; }
    }
}
