using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.LogBook
{
    public class CrewPassengerVM
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required") ]
        public string Name { get; set; }

        public int CompanyId { get; set; }

        public string EmailId { get; set; }
    }
}
