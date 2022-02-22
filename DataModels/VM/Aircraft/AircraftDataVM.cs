using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Aircraft
{
    public  class AircraftDataVM
    {
        public long Id { get; set; }
        public string TailNo { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public string ImagePath { get; set; }
        public string Year { get; set; }
        public string CompanyName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string Category { get; set; }
        public int TotalRecords { get; set; }

    }
}
