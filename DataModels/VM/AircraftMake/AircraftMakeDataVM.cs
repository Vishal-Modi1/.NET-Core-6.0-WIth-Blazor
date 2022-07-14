using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.AircraftMake
{
    public class AircraftMakeDataVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }

        public int TotalRecords { get; set; }
    }
}
