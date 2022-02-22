using DataModels.VM.Company;

namespace DataModels.VM.Aircraft
{
    public class AircraftFilterVM : CompanyFilterVM
    {
        public string TailNo { get; set; }
        public bool IsActive { get; set; }

        public string CompanyName { get; set; }
    }
}
