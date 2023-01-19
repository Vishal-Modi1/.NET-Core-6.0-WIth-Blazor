using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.VM.Scheduler
{
    public class FlightTagVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string TagName { get; set; }

        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}
