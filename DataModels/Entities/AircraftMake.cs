using System.ComponentModel.DataAnnotations;

namespace DataModels.Entities
{
    public class AircraftMake
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Make is required")]
        public string Name { get; set; }
    }
}
