﻿using DataModels.VM.Common;
using System;
using System.Collections.Generic;

namespace DataModels.VM.Company
{
    public class CompanyVM
    {
        public int Id { get; set; }

        public string Name { get; set; }
     
        public string Address { get; set; }

        public string TimeZone { get; set; }

        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public int TotalRecords { get; set; }
    }
}
