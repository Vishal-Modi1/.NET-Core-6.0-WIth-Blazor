using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.InstructorType
{
    public class InstructorTypeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Instructor Type")]
        public string Name { get; set; }

        public int TotalRecords { get; set; }

    }
}
