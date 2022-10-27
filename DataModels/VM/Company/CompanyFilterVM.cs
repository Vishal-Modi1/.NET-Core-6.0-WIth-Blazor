﻿using DataModels.VM.Common;
using System.Collections.Generic;

namespace DataModels.VM.Company
{
    public class CompanyFilterVM
    {
        public int CompanyId { get; set; }

        public List<DropDownValues> Companies { get; set; } = new List<DropDownValues> ();
        public List<DropDownValues> UserRoles { get; set; } = new List<DropDownValues>();

    }
}
