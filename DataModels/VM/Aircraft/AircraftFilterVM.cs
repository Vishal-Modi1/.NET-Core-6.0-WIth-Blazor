using DataModels.VM.Common;

namespace DataModels.VM.Aircraft
{
    public class AircraftFilterVM : CommonFilterVM
    {
        public string TailNo { get; set; }
        public bool IsActive { get; set; }

        public string CompanyName { get; set; }
    }
}
