using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.BillingConfigurations
{
    public class BillingConfigurationVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select billing type")]
        public string BillingFollows { get; set; }

        public int CompanyId { get; set; }

        public List<DropDownStringValues> BillingFollowsList { get; set; }
    }
}
