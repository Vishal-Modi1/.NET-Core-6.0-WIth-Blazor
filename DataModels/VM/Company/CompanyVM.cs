using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Company
{
    public class CompanyVM : CommonField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TimeZone { get; set; }
        public string Website { get; set; }
        public string PrimaryAirport { get; set; }
        public short? PrimaryServiceId { get; set; }
        public List<DropDownValues> PrimaryServicesList { get; set; }
        public string ContactNo { get; set; }
        public string Logo { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
        [NotMapped]
        public string LogoPath { get; set; }
        [NotMapped]
        public new long? CreatedBy { get; set; }
        public int TotalRecords { get; set; }
    }
}
