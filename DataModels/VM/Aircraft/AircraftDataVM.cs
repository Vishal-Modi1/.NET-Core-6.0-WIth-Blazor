using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Aircraft
{
    public class AircraftDataVM
    {
        public long Id { get; set; }
        public string TailNo { get; set; }
        public string ImageName { get; set; }

        public int CompanyId { get; set; }
        
        [NotMapped]
        public string ImagePath { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
        public byte AircraftStatusId { get; set; }
        public string AircraftStatus { get; set; }
        public string Indicator { get; set; }
        public string Year { get; set; }
        public string CompanyName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string Category { get; set; }
        public long OwnerId { get; set; }
        public bool IsLockedForUpdate { get; set; }
        public int TotalRecords { get; set; }

    }
}
