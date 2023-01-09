using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Scheduler
{
    public class FlightCategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Color { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public int? CompanyId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
