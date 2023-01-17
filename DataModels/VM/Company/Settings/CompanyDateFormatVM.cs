using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace DataModels.VM.Company.Settings
{
    public class CompanyDateFormatVM
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Date format is required")]
        public Int16 DateFormatId { get; set; }

        public List<DropDownSmallValues> DateFormatsList { get; set; }
    }
}
