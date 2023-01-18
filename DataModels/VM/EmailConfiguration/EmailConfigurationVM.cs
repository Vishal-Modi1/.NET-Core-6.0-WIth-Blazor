using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.EmailConfiguration
{
    public class EmailConfigurationVM
    {
        public long Id { get; set; }

        [Range(1, byte.MaxValue, ErrorMessage = "Email Type is required")]
        public int EmailTypeId { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public List<DropDownValues> EmailTypesList { get; set; }
    }
}
